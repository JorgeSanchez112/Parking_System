using Parking.Forms;
using Parking.Models;
using Parking.Services;
using Parking.Utils;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Parking
{
    public partial class FormHome : Form
    {
        // Windows API to hide caret in the invisible scanner textbox
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        // --- UI cache / timers ---
        private Bitmap _bgCache = null;
        private readonly Timer _resizeTimer;
        private const int RESIZE_DELAY_MS = 200;

        // --- App state ---
        private VehicleTypeCode currentTypeVehicle;
        private int _currentTicketId = -1;

        // --- Messages ---
        private const string messageLicensePlateEmpty = "El campo placa o identificacion es obligario";
        private const string messageOwnerIdEmpty = "El campo numero de identificacion es obligario";
        private const string messageVehicleSuccesfullyRegistered = "¡Vehiculo registrado exitosamente!";
        private const string messageJustDigits = "El número de identificación solo puede contener dígitos.";

        // --- Services (reused) ---
        private readonly TicketsService _ticketsService;
        private readonly VehiclesService _vehiclesService;
        private readonly VehiclesTypesService _vehicleTypesService;
        private readonly ParkingService _parkingService;
        private readonly BillsService _billsService;
        private readonly InfoParkingService _infoParkingService;

        public FormHome()
        {
            InitializeComponent();

            // instantiate reusable services once (single responsibility / reuse)
            _ticketsService = new TicketsService();
            _vehiclesService = new VehiclesService();
            _vehicleTypesService = new VehiclesTypesService();
            _parkingService = new ParkingService();
            _billsService = new BillsService();
            _infoParking_service_or_null_check(); // placeholder to avoid analyzer warnings
            _infoParkingService = new InfoParkingService();

            // form + painting settings
            this.DoubleBuffered = true;
            this.Shown += FormHome_shown;
            this.KeyPreview = true;
            SetDoubleBuffered(tableLayoutPanel1, true);

            // background resize debounce timer
            _resizeTimer = new Timer { Interval = RESIZE_DELAY_MS };
            _resizeTimer.Tick += ResizeTimer_Tick;
            tableLayoutPanel1.Paint += TableLayoutPanel1_Paint;
            tableLayoutPanel1.Resize += TableLayoutPanel1_Resize;

            // create initial background cache if size available
            UpdateBackgroundBitmap();

            // scanner textbox hidden but used to receive scanner input
            hideTextBoxScanner();
            textBoxScanner.GotFocus += textBoxScanner_GotFocus;
            textBoxScanner.TextChanged += textBoxScanner_TextChanged;

            // wire UI events that were previously done inline
            buttonFocuScanner.Click += buttonFocusScanner_Click;
        }

        // tiny no-op to silence any static analysis about declared field
        private void _infoParking_service_or_null_check() { /* no-op */ }

        private void FormHome_shown(object sender, EventArgs e)
        {
            loadParkingData();
            focusScanner();
        }

        // ==================== SCANNER HANDLING ====================
        // Scanner typically sends the barcode text + Enter key
        private void textBoxScanner_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;

            string scannedCode = textBoxScanner.Text?.Trim();
            if (string.IsNullOrEmpty(scannedCode))
            {
                focusScanner();
                return;
            }

            try
            {
                ProcessScannerCode(scannedCode);
            }
            catch (Exception ex)
            {
                // Safe fallback: show error and keep app responsive
                showTemporaryMessage(labelMessageError, $"ERROR PROCESANDO CÓDIGO: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        // Shared logic extracted from previous duplicated code
        private void ProcessScannerCode(string scannedCode)
        {
            int ticketId = _ticketsService.getIdByCodeBar(scannedCode);
            _currentTicketId = ticketId;

            var printData = _ticketsService.getPrintData(ticketId);
            if (printData == null)
            {
                showTemporaryMessage(labelMessageError, "CODIGO NO ENCONTRADO", 3000);
                ResetChargeUI();
                return;
            }

            ProcessPrintData(printData, ticketId);
        }

        // Consolidated logic: used by scanner and by manual validate (buttonValidateOwner_Click)
        private void ProcessPrintData(PrintData printData, int ticketId)
        {
            // verify vehicle registration (unless the type is Bike which uses owner id)
            bool vehicleStateActive = _vehiclesService.isVehicleStateActive(printData.LicensePlate);

            if (string.Equals(printData.CheckinState, CheckinsStateCode.facturado.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                showTemporaryMessage(labelMessageError, "ESTE TICKET YA FUE FACTURADO", 3000);
                ResetChargeUI();
                return;
            }

            if (!vehicleStateActive && !string.Equals(printData.VehicleType, VehicleTypeCode.Bike.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                showTemporaryMessage(labelMessageError, "EL VEHICULO NO ESTA REGISTRADO", 3000);
                ResetChargeUI();
                return;
            }

            // Show elapsed and cost based on checkin state
            if (string.Equals(printData.CheckinState, CheckinsStateCode.abierto.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                ShowChargeForOpen(printData);
                return;
            }

            if (string.Equals(printData.CheckinState, CheckinsStateCode.vip.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // find bill by checkin id (ticket -> checkin -> bill)
                int? checkinId = _ticketsService.getCheckinIdByTicketId(ticketId);
                Bills billData = null;
                if (checkinId.HasValue)
                {
                    billData = _billsService.getBillByCheckinId(checkinId.Value);
                }

                if (billData != null)
                {
                    ShowChargeForVIP(printData, billData);
                }
                else
                {
                    // fallback to generated cost if bill missing
                    ShowChargeForOpen(printData);
                }
            }
        }

        // helper to show time and generated cost
        private void ShowChargeForOpen(PrintData printData)
        {
            showElapsedTime(printData.MinutesElapsed);
            showCost(printData.TotalPayGenerated.ToString());
            showButtonGenerateInvoice();
            showButtonInvoicePaid();
        }

        // helper to show time and stored bill amount for VIP
        private void ShowChargeForVIP(PrintData printData, Bills billData)
        {
            showElapsedTime(printData.MinutesElapsed);
            showCost(billData.Total_pay.ToString());
            showButtonGenerateInvoice();
            showButtonInvoicePaid();
        }

        // ==================== BUTTON VALIDATE VEHICLE (manual search) ====================
        private void buttonValidateOwner_Click(object sender, EventArgs e)
        {
            try
            {
                string input = textBox2.Text?.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    showTemporaryMessage(labelMessageError, "POR FAVOR, INGRESAR UNA IDENTIFICACION DE VEHICULO VALIDA", 3000);
                    return;
                }

                int ticketId = input.All(char.IsDigit)
                    ? _ticketsService.getIdByOwnerId(input)
                    : _ticketsService.getIdByLicensePlate(input);

                _currentTicketId = ticketId;
                var printData = _ticketsService.getPrintData(ticketId);

                if (printData == null)
                {
                    showTemporaryMessage(labelMessageError, "VEHICULO NO ENCONTRADO", 3000);
                    ResetChargeUI();
                    return;
                }

                ProcessPrintData(printData, ticketId);
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR AL VALIDAR: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        // ==================== FOCUS & UI UTILITIES ====================
        private void buttonFocusScanner_Click(object sender, EventArgs e) => focusScanner();

        private void focusScanner()
        {
            try
            {
                textBoxScanner.Clear();
                if (textBoxScanner.CanFocus || !textBoxScanner.TabStop)
                    textBoxScanner.Focus();
            }
            catch
            {
                // swallow - not critical
            }
        }

        // Reset UI elements for charge/time/cost/buttons
        private void ResetChargeUI()
        {
            hideElapsedTime();
            hideCost();
            hideButtonGenerateInvoice();
            hideButtonInvoicePaid();
        }

        // ==================== BACKGROUND RENDERING (unchanged, optimized minor) ===
        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            if (_bgCache != null)
            {
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImageUnscaled(_bgCache, 0, 0);
                return;
            }

            var img = Properties.Resources.imagen_estadio;
            if (img != null)
                e.Graphics.DrawImage(img, new Rectangle(0, 0, tableLayoutPanel1.Width, tableLayoutPanel1.Height));
        }

        private void TableLayoutPanel1_Resize(object sender, EventArgs e)
        {
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private void ResizeTimer_Tick(object sender, EventArgs e)
        {
            _resizeTimer.Stop();
            UpdateBackgroundBitmap();
            tableLayoutPanel1.Invalidate();
        }

        private void UpdateBackgroundBitmap()
        {
            var img = Properties.Resources.imagen_estadio;
            if (img == null) return;
            var w = tableLayoutPanel1.ClientSize.Width;
            var h = tableLayoutPanel1.ClientSize.Height;
            if (w <= 0 || h <= 0) return;
            if (_bgCache != null && _bgCache.Width == w && _bgCache.Height == h) return;

            _bgCache?.Dispose();
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.None;
                g.DrawImage(img, new Rectangle(0, 0, w, h));
            }
            _bgCache = bmp;
        }

        // give non-public controls double-buffering
        private void SetDoubleBuffered(Control c, bool enabled)
        {
            PropertyInfo prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop?.SetValue(c, enabled, null);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _bgCache?.Dispose();
            _resizeTimer?.Dispose();
        }

        // ==================== NAVIGATION & OTHER FORMS (unchanged) ====================
        private void ajsutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSettings = new FormSettings();
            formSettings.FormClosed += (s, args) => loadParkingData();
            formSettings.ShowDialog();
            focusScanner();
        }

        private void listaVehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var history = new FormVehiculesHistory();
            history.FormClosed += (s, args) => focusScanner();
            history.Show();
        }

        // ==================== VEHICLE SELECTION UI ====================
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SetVehicleInputUI("INGRESAR CARRO", 6, VehicleTypeCode.Car,
                carClickedStyle, bikeDefaultStyle, motorBikeDefaultStyle, carDefaultStyle);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SetVehicleInputUI("INGRESAR MOTO", 6, VehicleTypeCode.Motorbike,
                motorBikeClickedStyle, carDefaultStyle, bikeDefaultStyle, motorBikeDefaultStyle);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SetVehicleInputUI("INGRESAR NUMERO DE IDENTIFICACION", 10, VehicleTypeCode.Bike,
                bikeClickedStyle, carDefaultStyle, motorBikeDefaultStyle, bikeDefaultStyle);
        }

        // small helper to set UI when selecting vehicle type
        private void SetVehicleInputUI(string labelText, int maxLength, VehicleTypeCode type,
            Action clickedStyle, Action defaultStyle1, Action defaultStyle2, Action defaultReset)
        {
            setValueToLabel6(labelText);
            showLabel6();
            ValueBoldToLabel6();
            clearTextBox1();
            setTexBox1MaxLength(maxLength);
            showTextBox1();
            showButtonSaveVehicle();
            clickedStyle?.Invoke();
            defaultStyle1?.Invoke();
            defaultStyle2?.Invoke();
            hideElementsVip();
            currentTypeVehicle = type;
        }

        // ==================== VEHICLE INPUT EVENTS ====================
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool vehicleWithMotor = _vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle);

            if (vehicleWithMotor)
            {
                int selStart = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.ToUpper();
                textBox1.SelectionStart = Math.Min(selStart, textBox1.Text.Length);
            }

            if (currentTypeVehicle == VehicleTypeCode.Bike)
            {
                bool allDigits = !string.IsNullOrEmpty(textBox1.Text) && textBox1.Text.All(char.IsDigit);
                buttonSaveVehicle.Enabled = allDigits;
            }
            else
            {
                buttonSaveVehicle.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text);
            }
        }

        private void textBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            if (currentTypeVehicle != VehicleTypeCode.Bike) return;
            string ownerId = textBox1.Text?.Trim();
            if (!string.IsNullOrEmpty(ownerId) && !ownerId.All(char.IsDigit))
            {
                showTemporaryMessage(labelMessageError, messageJustDigits, 3000);
            }
        }

        // ==================== VEHICLE SAVE (consolidated & validated) ====================
        private void buttonSaveVehicle_Click(object sender, EventArgs e)
        {
            try
            {
                SaveVehicleFlow(isVip: false);
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR AL REGISTRAR: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        // Save vehicle for VIP button
        private void guardarVIP_Click(object sender, EventArgs e)
        {
            try
            {
                SaveVehicleFlow(isVip: true);
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR VIP: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        // Unified save logic for normal and VIP checkins
        private void SaveVehicleFlow(bool isVip)
        {
            bool vehicleWithMotor = _vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle);

            string licensePlate = vehicleWithMotor ? valueToSaveUsingVehicleWithMotor(true) : null;
            string ownerId = !vehicleWithMotor ? valueToSaveUsingVehicleWithoutMotor(false) : null;

            // Basic validations
            if (vehicleWithMotor && string.IsNullOrWhiteSpace(licensePlate))
            {
                showTemporaryMessage(labelMessageError, messageLicensePlateEmpty, 3000);
                return;
            }

            if (!vehicleWithMotor && string.IsNullOrWhiteSpace(ownerId))
            {
                showTemporaryMessage(labelMessageError, messageOwnerIdEmpty, 3000);
                return;
            }

            if (vehicleWithMotor && !_vehiclesService.isTextBoxLengthValid(textBox1, 6))
            {
                showTemporaryMessage(labelMessageError, "LA PLACA DEBE TENER AL MENOS 6 CARACTERES", 3000);
                return;
            }

            // Specific plate format validations (enum usage)
            if (currentTypeVehicle == VehicleTypeCode.Motorbike)
            {
                string platePattern = @"^[A-Z]{3}[0-9]{2}[A-Z]{1}$";
                string normalized = (licensePlate ?? "").ToUpper();
                if (!System.Text.RegularExpressions.Regex.IsMatch(normalized, platePattern))
                {
                    showTemporaryMessage(labelMessageError, "FORMATO DE PLACA TIPO MOTO INVÁLIDO. EJEMPLO: ABC12D", 4000);
                    return;
                }
                licensePlate = normalized;
                textBox1.Text = normalized; // enforce uppercase in UI
            }

            if (currentTypeVehicle == VehicleTypeCode.Car)
            {
                string platePattern = @"^[A-Z]{3}[0-9]{3}$";
                string normalized = (licensePlate ?? "").ToUpper();
                if (!System.Text.RegularExpressions.Regex.IsMatch(normalized, platePattern))
                {
                    showTemporaryMessage(labelMessageError, "FORMATO DE PLACA TIPO CARRO INVÁLIDO. EJEMPLO: ABC123", 4000);
                    return;
                }
                licensePlate = normalized;
                textBox1.Text = normalized;
            }

            // Detect existing vehicle
            Vehicles existingVehicle = null;
            if (vehicleWithMotor && _vehiclesService.validateLicensePlateExist(licensePlate))
                existingVehicle = _vehiclesService.getByLicensePlate(licensePlate);
            else if (!vehicleWithMotor && _vehiclesService.validateOwnerExist(ownerId))
                existingVehicle = _vehiclesService.getByOwnerId(ownerId);

            if (existingVehicle == null)
            {
                // Create new vehicle + checkin (+bill if VIP)
                Vehicles newVehicle = new Vehicles
                {
                    Type_id = _vehicleTypesService.GetId(currentTypeVehicle),
                    License_plate = licensePlate,
                    Owner_id = ownerId,
                    State = VehicleStateCode.activo.ToString()
                };

                Checkins checkin = new Checkins
                {
                    EntryTime = DateTime.Now,
                    State = isVip ? CheckinsStateCode.vip.ToString() : CheckinsStateCode.abierto.ToString()
                };

                Tickets ticket = new Tickets
                {
                    Parking_id = 1,
                    Codebar = BarcodeHelper.GenerateUniqueCodebar(),
                    Release_date = DateTime.Now
                };

                if (isVip)
                {
                    String regexNumeric = @"^\d+$";

                    if (String.IsNullOrEmpty(textBoxSpecialFee.Text))
                    {
                        showTemporaryMessage(labelMessageError, "VIP: EL CAMPO COSTO NO PUEDE ESTAR VACIO", 4000);
                        return;
                    }

                    if (!Regex.IsMatch(textBoxSpecialFee.Text, regexNumeric))
                    {
                        showTemporaryMessage(labelMessageError, "VIP: EL CAMPO SOLO PERMITE NUMEROS", 4000);
                        return;
                    }

                    Bills bill = new Bills
                    {
                        Parking_id = 1,
                        Total_pay = int.TryParse(textBoxSpecialFee.Text, out var fee) ? fee : 0,
                        Checkin_Time = DateTime.Now
                    };

                    _parking_service_register_vehicle_with_bill(newVehicle, checkin, ticket, bill);
                }
                else
                {
                    _parking_service_register_vehicle_checkin(newVehicle, checkin, ticket);
                }
            }
            else
            {
                // existing vehicle flows
                if (existingVehicle.State == VehicleStateCode.activo.ToString())
                {
                    showTemporaryMessage(labelMessageError, "ESTE VEHICULO YA SE ENCUENTRA REGISTRADO", 3000);
                    return;
                }

                if (existingVehicle.Type_id != _vehicleTypesService.GetId(currentTypeVehicle))
                {
                    showTemporaryMessage(labelMessageError, "ESTE VEHICULO ESTA ALMACENADO EN EL SISTEMA CON UN TIPO DIFERENTE AL SELECCIONADO", 3000);
                    return;
                }

                Checkins checkin = new Checkins
                {
                    EntryTime = DateTime.Now,
                    State = isVip ? CheckinsStateCode.vip.ToString() : CheckinsStateCode.abierto.ToString()
                };

                Tickets ticket = new Tickets
                {
                    Parking_id = 1,
                    Codebar = BarcodeHelper.GenerateUniqueCodebar(),
                    Release_date = DateTime.Now
                };

                if (isVip)
                {

                    String regexNumeric = @"^\d+$";

                    if (String.IsNullOrEmpty(textBoxSpecialFee.Text))
                    {
                        showTemporaryMessage(labelMessageError, "VIP: EL CAMPO COSTO NO PUEDE ESTAR VACIO", 4000);
                        return;
                    }

                    if (!Regex.IsMatch(textBoxSpecialFee.Text, regexNumeric))
                    {
                        showTemporaryMessage(labelMessageError, "VIP: EL CAMPO SOLO PERMITE NUMEROS", 4000);
                        return;
                    }

                    Bills bill = new Bills
                    {
                        Parking_id = 1,
                        Total_pay = int.TryParse(textBoxSpecialFee.Text, out var fee) ? fee : 0,
                        Checkin_Time = DateTime.Now
                    };

                    _parking_service_register_checkin_with_bill(existingVehicle.Id, checkin, ticket, bill);
                    changeVehicleStateToActive(String.IsNullOrEmpty(existingVehicle.License_plate) ? existingVehicle.Owner_id : existingVehicle.License_plate);
                }
                else
                {
                    _parking_service_register_checkin(existingVehicle.Id, checkin, ticket);
                    changeVehicleStateToActive(String.IsNullOrEmpty(existingVehicle.License_plate) ? existingVehicle.Owner_id : existingVehicle.License_plate);
                }
            }

            // success & follow-up
            showTemporarySuccesMessage(labelMessageError, messageVehicleSuccesfullyRegistered, 3000);
            printTicket();
        }

        // small wrappers to avoid repeating service method names inline (keeps single responsibility)
        private void _parking_service_register_vehicle_checkin(Vehicles vehicle, Checkins checkin, Tickets ticket)
            => _parkingService.RegisterVehicleCheckin(vehicle, checkin, ticket);

        private void _parking_service_register_checkin(int vehicleId, Checkins checkin, Tickets ticket)
            => _parkingService.RegisterCheckin(vehicleId, checkin, ticket);

        private void _parking_service_register_vehicle_with_bill(Vehicles vehicle, Checkins checkin, Tickets ticket, Bills bill)
            => _parkingService.RegisterVehicleCheckinWithBill(vehicle, checkin, ticket, bill);

        private void _parking_service_register_checkin_with_bill(int vehicleId, Checkins checkin, Tickets ticket, Bills bill)
            => _parkingService.RegisterCheckinWithBill(vehicleId, checkin, ticket, bill);

        // ==================== BILLING ====================
        private void buttonGenerateBill_Click(object sender, EventArgs e)
        {
            try
            {
                insertBill();
                ResetChargeUI();
                printBill(true);
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR AL GENERAR FACTURA: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        private void buttonPayed_Click(object sender, EventArgs e)
        {
            try
            {
                insertBill();
                ResetChargeUI();
                printBill(false);
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR AL MARCAR COMO PAGADO: {ex.Message}", 3000);
            }
            finally
            {
                focusScanner();
            }
        }

        private void insertBill()
        {
            var printData = _ticketsService.getPrintData(_currentTicketId);
            if (printData == null) throw new InvalidOperationException("No hay datos de impresión para el ticket actual.");

            string licensePlate = printData.VehicleInfo;

            if (string.Equals(printData.CheckinState, CheckinsStateCode.vip.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                int? checkinId = _ticketsService.getCheckinIdByTicketId(_currentTicketId);
                if (!checkinId.HasValue) throw new InvalidOperationException("No existe checkin asociado al ticket VIP.");

                Bills oldBill = _billsService.getBillByCheckinId(checkinId.Value);
                if (oldBill == null) throw new InvalidOperationException("No existe factura previa para este checkin VIP.");

                var updated = new Bills
                {
                    Id = oldBill.Id,
                    Checkin_id = oldBill.Checkin_id,
                    Parking_id = oldBill.Parking_id,
                    Total_pay = oldBill.Total_pay,
                    Release_date = convertElapsedValueToTotalMinutes(printData.EntryTime),
                    Checkin_Time = oldBill.Checkin_Time
                };

                _billsService.updateBill(updated);
            }
            else
            {
                var newBill = new Bills
                {
                    Checkin_id = printData.CheckinId,
                    Parking_id = 1,
                    Total_pay = int.TryParse(labelCostValue.Text, out var total) ? total : 0,
                    Release_date = convertElapsedValueToTotalMinutes(printData.EntryTime),
                    Checkin_Time = printData.EntryTime
                };

                _billsService.createBill(newBill);
            }

            changeCheckinStateToFacturado(printData.CheckinId);
            changeVehicleStateToInactive(licensePlate);
        }

        // ==================== UI SHOW/HIDE HELPERS (kept names to preserve designer references) ===
        private void showTextBox1() => textBox1.Visible = true;
        private void showButtonSaveVehicle() => buttonSaveVehicle.Visible = true;
        private void setTexBox1MaxLength(int limit) => textBox1.MaxLength = limit;
        private void clearTextBox1() => textBox1.Clear();
        private void setValueToLabel6(string txt) => label6.Text = txt;
        private void ValueBoldToLabel6() => label6.Font = new Font(label6.Font, FontStyle.Bold);
        private void showLabel6() => label6.Visible = true;

        private void carClickedStyle()
        {
            flowLayoutPanel2.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox1.BackColor = Color.Navy;
        }

        private void motorBikeClickedStyle()
        {
            flowLayoutPanel4.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.BackColor = Color.Navy;
        }

        private void bikeClickedStyle()
        {
            flowLayoutPanel1.BorderStyle = BorderStyle.Fixed3D;
            pictureBox3.BorderStyle = BorderStyle.Fixed3D;
            pictureBox3.BackColor = Color.Bisque;
        }

        private void carDefaultStyle()
        {
            flowLayoutPanel2.BorderStyle = BorderStyle.None;
            pictureBox1.BorderStyle = BorderStyle.None;
            pictureBox1.BackColor = Color.Transparent;
        }

        private void motorBikeDefaultStyle()
        {
            flowLayoutPanel4.BorderStyle = BorderStyle.None;
            pictureBox2.BorderStyle = BorderStyle.None;
            pictureBox2.BackColor = Color.Transparent;
        }

        private void bikeDefaultStyle()
        {
            flowLayoutPanel1.BorderStyle = BorderStyle.None;
            pictureBox3.BorderStyle = BorderStyle.None;
            pictureBox3.BackColor = Color.Transparent;
        }

        private void FormHome_Load(object sender, EventArgs e) { }

        private void loadParkingData()
        {
            try
            {
                var data = _infoParkingService.getAllInfoParking();
                if (data != null && data.Count > 0)
                    labelTitleParking.Text = data[0].Name_parking;
            }
            catch
            {
                // ignore errors loading settings; UI will remain functional
            }
        }

        private void printTicket()
        {
            try
            {
                int ticketId = _ticketsService.getLastIndex();
                var printData = _ticketsService.getPrintData(ticketId);
                if (printData != null)
                {
                    PrintHelper.printTicket(printData);
                    showTemporarySuccesMessage(labelMessageError, "IMPRIMIENDO TICKET", 3000);
                }
                else
                {
                    showTemporaryMessage(labelMessageError, "ALGO FUE MAL AL GENERAR EL TICKET", 300);
                }
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR IMPRESIÓN: {ex.Message}", 3000);
            }
        }

        private void printBill(bool shouldPrint)
        {
            try
            {
                int billId = _billsService.getLastIndex();
                var printData = _billsService.getPrintData(billId);
                if (printData != null)
                {
                    if (shouldPrint)
                    {
                        PrintHelper.printInvoice(printData);
                        showTemporarySuccesMessage(labelMessageError, "IMPRIMIENDO FACTURA", 3000);
                    }
                    else
                    {
                        showTemporarySuccesMessage(labelMessageError, "FACTURADO", 3000);
                    }
                }
                else
                {
                    showTemporaryMessage(labelMessageError, "ALGO FUE MAL AL GENERAR LA FACTURA", 300);
                }
            }
            catch (Exception ex)
            {
                showTemporaryMessage(labelMessageError, $"ERROR IMPRESIÓN: {ex.Message}", 3000);
            }
        }

        // helpers used earlier
        private string valueToSaveUsingVehicleWithMotor(bool vehicleWithMotor)
            => !vehicleWithMotor ? null : (string.IsNullOrWhiteSpace(textBox1.Text) ? null : textBox1.Text);

        private string valueToSaveUsingVehicleWithoutMotor(bool vehicleWithMotor)
            => vehicleWithMotor ? null : (string.IsNullOrWhiteSpace(textBox1.Text) ? null : textBox1.Text);

        // async UI message helpers (kept behavior but centralized)
        private async void showTemporaryMessage(Label label, string message, int milliseconds)
        {
            label.Text = message;
            label.Visible = true;
            label.ForeColor = Color.Red;
            label.BackColor = Color.Black;
            await Task.Delay(milliseconds);
            label.Visible = false;
        }

        private async void showTemporarySuccesMessage(Label label, string message, int milliseconds)
        {
            label.Text = message;
            label.Visible = true;
            label.ForeColor = Color.Blue;
            label.BackColor = Color.White;
            await Task.Delay(milliseconds);
            label.Visible = false;
        }

        private void showElapsedTime(int minutes)
        {
            label4.Visible = true;
            labelElapsedValue.Visible = true;
            labelElapsedValue.Text = $"{minutes / 60}h {minutes % 60}m";
        }

        private void showCost(string value)
        {
            label7.Visible = true;
            labelCostValue.Visible = true;
            labelCostValue.Text = value;
        }

        private void showButtonGenerateInvoice() => buttonGenerateBill.Visible = true;
        private void showButtonInvoicePaid() => buttonPayed.Visible = true;
        private void hideButtonSaveVehicle() => buttonSaveVehicle.Visible = false;

        private void showVipElements()
        {
            labelTextSpecialFee.Visible = true;
            textBoxSpecialFee.Visible = true;
            guardarVIP.Visible = true;

            tableLayoutPanel1.SetRow(labelTextSpecialFee, 12);
            tableLayoutPanel1.SetColumn(labelTextSpecialFee, 11);

            tableLayoutPanel1.SetRow(textBoxSpecialFee, 12);
            tableLayoutPanel1.SetColumn(textBoxSpecialFee, 12);

            tableLayoutPanel1.SetRow(guardarVIP, 12);
            tableLayoutPanel1.SetColumn(guardarVIP, 13);
        }

        private void hideElementsVip()
        {
            labelTextSpecialFee.Visible = false;
            textBoxSpecialFee.Visible = false;
            guardarVIP.Visible = false;
        }

        private void hideElapsedTime()
        {
            label4.Visible = false;
            labelElapsedValue.Visible = false;
            labelElapsedValue.Text = "0";
        }

        private void hideCost()
        {
            label7.Visible = false;
            labelCostValue.Visible = false;
            labelCostValue.Text = "0";
        }

        private void hideButtonGenerateInvoice() => buttonGenerateBill.Visible = false;
        private void hideButtonInvoicePaid() => buttonPayed.Visible = false;

        // Keep scanner TextBox hidden but receiving input
        private void hideTextBoxScanner()
        {
            textBoxScanner.Size = new Size(1, 1);
            textBoxScanner.Location = new Point(-100, -100);
            textBoxScanner.TabStop = false;
            textBoxScanner.BorderStyle = BorderStyle.None;
            textBoxScanner.KeyPress += textBoxScanner_KeyPress;
        }

        private void textBoxScanner_GotFocus(object sender, EventArgs e) => HideCaret(textBoxScanner.Handle);
        private void textBoxScanner_TextChanged(object sender, EventArgs e) => HideCaret(textBoxScanner.Handle);

        private void changeVehicleStateToInactive(string licensePlate)
        {
            String regexBike = @"^\d+$";

            Vehicles v = new Vehicles();

            if (Regex.IsMatch(licensePlate, regexBike))
            {
                v.Owner_id = licensePlate;
                v.State = VehicleStateCode.inactivo.ToString();
            }
            else
            {
                v.License_plate = licensePlate;
                v.State = VehicleStateCode.inactivo.ToString();
            }

                _vehiclesService.setVehicleState(v);
        }

        private void changeVehicleStateToActive(string licensePlate)
        {

            String regexBike = @"^\d+$";

            Vehicles v = new Vehicles();

            if (Regex.IsMatch(licensePlate, regexBike))
            {
                v.Owner_id = licensePlate;
                v.State = VehicleStateCode.activo.ToString();
            }
            else
            {
                v.License_plate = licensePlate;
                v.State = VehicleStateCode.activo.ToString();
            }

            _vehiclesService.setVehicleStateWhatever(v);
        }

        private void changeCheckinStateToFacturado(int checkinId)
        {
            Checkins c = new Checkins { Id = checkinId, State = CheckinsStateCode.facturado.ToString() };
            CheckinsService svc = new CheckinsService();
            svc.setCheckinState(c);
        }

        private DateTime convertElapsedValueToTotalMinutes(DateTime entryTime)
        {
            string elapsedText = labelElapsedValue.Text;
            int hours = int.Parse(elapsedText.Split('h')[0].Trim());
            int minutes = int.Parse(elapsedText.Split('h')[1].Replace("m", "").Trim());
            int totalMinutes = (hours * 60) + minutes;
            return entryTime.AddMinutes(totalMinutes);
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            int selStart = textBox2.SelectionStart;
            textBox2.Text = textBox2.Text.ToUpper();
            textBox2.SelectionStart = Math.Min(selStart, textBox2.Text.Length);
        }

        private void buttonFocuScanner_Click(object sender, EventArgs e) { /* reserved for UI */ }
        private void textBoxScanner_TextChanged_1(object sender, EventArgs e) { /* reserved for UI */ }

        private void buttonVIP_Click(object sender, EventArgs e)
        {
            showVipElements();
            hideButtonSaveVehicle();
        }

        // Unused designer event stubs kept for compatibility
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void labelMessageError_Click(object sender, EventArgs e) { }
        private void labelElapsedValue_Click(object sender, EventArgs e) { }
        private void textBoxCodeBarScanner_TextChanged(object sender, EventArgs e) { }
    }
}

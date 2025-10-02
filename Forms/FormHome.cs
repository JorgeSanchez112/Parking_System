using Parking.Forms;
using Parking.Models;
using Parking.Services;
using Parking.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking
{
    public partial class FormHome : Form
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private Bitmap _bgCache = null;
        private Timer _resizeTimer = null;
        private const int RESIZE_DELAY_MS = 200;


        private VehicleTypeCode currentTypeVehicle;

        private int _currentTicketId = -1;

        private const String messageLicensePlateEmpty = "El campo placa es obligario";
        private const String messageOwnerIdEmpty = "El campo numero de identificacion es obligario";
        private const String messageVehicleSuccesfullyRegistered = "¡Vehiculo registrado exitosamente!";
        private const String messageJustDigits = "El número de identificación solo puede contener dígitos.";

        public FormHome()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Shown += FormHome_shown;
            this.KeyPreview = true;

            // Enable double buffering
            this.DoubleBuffered = true;
            SetDoubleBuffered(tableLayoutPanel1, true);

            // Events
            tableLayoutPanel1.Paint += TableLayoutPanel1_Paint;
            tableLayoutPanel1.Resize += TableLayoutPanel1_Resize;

            // Create the timer (only once)
            _resizeTimer = new Timer();
            _resizeTimer.Interval = RESIZE_DELAY_MS;
            _resizeTimer.Tick += ResizeTimer_Tick;

            // Create the initial bitmap (if the control already has a size)
            UpdateBackgroundBitmap();

            hideTextBoxScanner();
            textBoxScanner.GotFocus += textBoxScanner_GotFocus;
            textBoxScanner.TextChanged += textBoxScanner_TextChanged;

            buttonFocuScanner.Click += buttonFocusScanner_Click;
        }

        private void FormHome_shown(Object sender, EventArgs e)
        {
            loadParkingData();
            focusScanner(); // always start focused on scanner input
        }

        // ==================== SCANNER HANDLING ====================

        // When the scanner sends data it will go into textBoxScanner. Scanners typically end with Enter.
        private void textBoxScanner_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) // scanner usually sends Enter (char 13) at the end
            {
                string scannedCode = textBoxScanner.Text.Trim();

                if (!string.IsNullOrEmpty(scannedCode))
                {
                    TicketsService _ticketService = new TicketsService();
                    VehiclesTypesService _vehicleType = new VehiclesTypesService();
                    VehiclesService _vehiclesService = new VehiclesService();
                    ParkingService _parkingService = new ParkingService();

                    

                    int ticketId = _ticketService.getIdByCodeBar(scannedCode);
                    _currentTicketId = ticketId;  // save last number scanned

                    var _printData = _ticketService.getPrintData(ticketId);

                    if (_printData != null)
                    {
                        bool vehicleStateActive = _vehiclesService.isVehicleStateActive(_printData.LicensePlate);

                        if (_printData.CheckinState == "facturado")
                        {
                            showTemporaryMessage(labelMessageError, "ESTE TICKET YA FUE FACTURADO", 3000);
                            hideElapsedTime();
                            hideCost();
                            hideButtonGenerateInvoice();
                            hideButtonInvoicePaid();
                            return;
                        }

                        if (vehicleStateActive == false && _printData.VehicleType != VehicleTypeCode.Bike.ToString())
                        {
                            showTemporaryMessage(labelMessageError, "EL VEHICULO NO ESTA REGISTRADO", 3000);
                            hideElapsedTime();
                            hideCost();
                            hideButtonGenerateInvoice();
                            hideButtonInvoicePaid();
                            return;
                        }

                        showElapsedTime(_printData.MinutesElapsed);
                        showCost(_printData.TotalPayGenerated.ToString());
                        showButtonGenerateInvoice();
                        showButtonInvoicePaid();
                    }
                    else
                    {
                        showTemporaryMessage(labelMessageError, "CODIGO NO ENCONTRADO", 3000);
                        hideElapsedTime();
                        hideCost();
                        hideButtonGenerateInvoice();
                        hideButtonInvoicePaid();
                    }
                }

                focusScanner(); // re-focus scanner for the next scan
            }
        }

        // Button event: user clicks this to force focus into the scanner TextBox
        private void buttonFocusScanner_Click(object sender, EventArgs e)
        {
            focusScanner();
        }

        // Centralized method to set focus to scanner input
        private void focusScanner()
        {
            try
            {
                textBoxScanner.Clear();// clear the texbox

                // Ensure control can take focus
                if (textBoxScanner.CanFocus || textBoxScanner.TabStop == false)
                {
                    textBoxScanner.Focus();
                }
            }
            catch
            {
                // swallow any unlikely focus exceptions - keep app resilient
            }
        }

        // ==================== UI EVENT HANDLERS (unchanged) ====================
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
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void labelMessageError_Click(object sender, EventArgs e) { }
        private void labelElapsedValue_Click(object sender, EventArgs e) { }
        private void textBoxCodeBarScanner_TextChanged(object sender, EventArgs e) { }


        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Draw the cached image (if it exists)
            if (_bgCache != null)
            {
                // If it’s already exactly the right size, draw without scaling for maximum speed
                e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.DrawImageUnscaled(_bgCache, 0, 0);
            }
            else
            {
                // Fallback: draw resource directly (not recommended for long-term performance)
                var img = Properties.Resources.imagen_estadio;
                if (img != null)
                    e.Graphics.DrawImage(img, new Rectangle(0, 0, tableLayoutPanel1.Width, tableLayoutPanel1.Height));
            }
        }

        // Don’t recalculate immediately on each resize: use a timer for "debounce"
        private void TableLayoutPanel1_Resize(object sender, EventArgs e)
        {
            // Restart the timer: if the user keeps dragging, we don’t recalculate yet
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private void ResizeTimer_Tick(object sender, EventArgs e)
        {
            _resizeTimer.Stop();
            UpdateBackgroundBitmap();
            tableLayoutPanel1.Invalidate(); // force quick repaint using the cached image
        }

        private void UpdateBackgroundBitmap()
        {
            var img = Properties.Resources.imagen_estadio;
            if (img == null) return;

            var w = tableLayoutPanel1.ClientSize.Width;
            var h = tableLayoutPanel1.ClientSize.Height;

            if (w <= 0 || h <= 0) return;

            // If it already exists and has the right size, do nothing
            if (_bgCache != null && _bgCache.Width == w && _bgCache.Height == h) return;

            // Release previous cache
            _bgCache?.Dispose();

            // Create new scaled version (only once)
            // Use reasonable scaling quality: HighQualityBicubic is good but a bit slower.
            var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.None;

                // Draw the scaled image across the whole panel
                g.DrawImage(img, new Rectangle(0, 0, w, h));
            }

            _bgCache = bmp;
        }

        // Utility to enable double buffering in controls that don’t expose the property
        private void SetDoubleBuffered(Control c, bool enabled)
        {
            // Reflection because the property may be non-public
            PropertyInfo prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop != null) prop.SetValue(c, enabled, null);
        }

        // Release resources when closing the form
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _bgCache?.Dispose();
            _resizeTimer?.Dispose();
        }


        private void ajsutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();

            // Reload parking data once the settings form is closed
            formSettings.FormClosed += (s, args) => loadParkingData();

            formSettings.ShowDialog();
            focusScanner(); // return focus to scanner after closing the modal settings
        }

        private void listaVehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVehiculesHistory formVehiculesHistory = new FormVehiculesHistory();
            // Ensure focus returns to scanner when the history form is closed
            formVehiculesHistory.FormClosed += (s, args) => focusScanner();
            formVehiculesHistory.Show();
        }

        // ==================== VEHICLE SELECTION ====================
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            setValueToLabel6("INGRESAR CARRO");
            showLabel6();
            ValueBoldToLabel6();
            clearTextBox1();
            setTexBox1MaxLength(6);
            showTextBox1();
            showButtonSaveVehicle();
            carClickedStyle();
            motorBikeDefaultStyle();
            bikeDefaultStyle();
            currentTypeVehicle = VehicleTypeCode.Car;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            setValueToLabel6("INGRESAR MOTO");
            showLabel6();
            ValueBoldToLabel6();
            clearTextBox1();
            setTexBox1MaxLength(6);
            showTextBox1();
            showButtonSaveVehicle();
            motorBikeClickedStyle();
            carDefaultStyle();
            bikeDefaultStyle();
            currentTypeVehicle = VehicleTypeCode.Motorbike;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            setValueToLabel6("INGRESAR NUMERO DE IDENTIFICACION");
            showLabel6();
            ValueBoldToLabel6();
            clearTextBox1();
            setTexBox1MaxLength(10);
            showTextBox1();
            showButtonSaveVehicle();
            bikeClickedStyle();
            carDefaultStyle();
            motorBikeDefaultStyle();
            currentTypeVehicle = VehicleTypeCode.Bike;
        }

        // ==================== VEHICLE INPUT ====================
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            VehiclesService _vehiclesService = new VehiclesService();
            bool vehicleWithMotor = _vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle);

            // If the vehicle has a motor (car/moto) convert to uppercase
            if (vehicleWithMotor)
            {
                int selStart = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.ToUpper();
                textBox1.SelectionStart = selStart;

            }

            // Enable/disable Save button depending on type
            if (currentTypeVehicle == VehicleTypeCode.Bike) // Only bikes
            {
                bool allDigits = !string.IsNullOrEmpty(textBox1.Text) && textBox1.Text.All(char.IsDigit);
                buttonSaveVehicle.Enabled = allDigits;
            }
            else // Cars and motorbikes
            {
                buttonSaveVehicle.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text);
            }
        }

        private void textBox1KeyPress(object sender, KeyPressEventArgs e)
        {
            if (currentTypeVehicle == VehicleTypeCode.Bike)
            {
                string ownerId = textBox1.Text?.Trim();
                if (!ownerId.All(char.IsDigit))
                {
                    showTemporaryMessage(labelMessageError, messageJustDigits, 3000);
                    return;
                }
            }
        }

        // ==================== VEHICLE SAVE ====================
        private void buttonSaveVehicle_Click(object sender, EventArgs e)
        {
            ParkingService _parkingService = new ParkingService();
            VehiclesTypesService _vehiclesTypeService = new VehiclesTypesService();
            VehiclesService _vehiclesService = new VehiclesService();

            bool vehicleWithMotor = _vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle);

            string licensePlate = vehicleWithMotor ? valueToSaveUsingVehicleWithMotor(true) : null;
            string ownerId = !vehicleWithMotor ? valueToSaveUsingVehicleWithoutMotor(false) : null;

            // === Validations ===
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


            // === Main Flow ===
            Vehicles existingVehicle = null;
            if (vehicleWithMotor && _vehiclesService.validateLicensePlateExist(licensePlate))
            {
                existingVehicle = _vehiclesService.getByLicensePlate(licensePlate);
            }
            else if (!vehicleWithMotor && _vehiclesService.validateOwnerExist(ownerId))
            {
                existingVehicle = _vehiclesService.getByOwnerId(ownerId);
            }

            if (existingVehicle == null)
            {
                // Crear vehículo nuevo
                Vehicles _vehicle = new Vehicles
                {
                    Type_id = _vehiclesTypeService.GetId(currentTypeVehicle),
                    License_plate = licensePlate,
                    Owner_id = ownerId,
                    State = VehicleStateCode.activo.ToString()
                };

                Checkins _checkin = new Checkins
                {
                    EntryTime = DateTime.Now,
                    State = CheckinsStateCode.abierto.ToString(),
                };

                Tickets _ticket = new Tickets
                {
                    Parking_id = 1,
                    Codebar = BarcodeHelper.GenerateUniqueCodebar(),
                    Release_date = DateTime.Now
                };

                _parkingService.RegisterVehicleCheckin(_vehicle, _checkin, _ticket);
            }
            else
            {
                // Validar que no esté inactivo
                if (existingVehicle.State == VehicleStateCode.activo.ToString())
                {
                    showTemporaryMessage(labelMessageError, "ESTE VEHICULO YA SE ENCUENTRA REGISTRADO", 3000);
                    return;
                }

                // Validar que la placa sea unica
                if (existingVehicle.Type_id != _vehiclesTypeService.GetId(currentTypeVehicle))
                {
                    showTemporaryMessage(labelMessageError, "ESTE VEHICULO ESTA ALMACENADO EN EL SISTEMA CON UN TIPO DIFERENTE AL SELECCIONADO", 3000);
                    return;
                }

                // Crear solo checkin + ticket
                Checkins _checkin = new Checkins
                {
                    EntryTime = DateTime.Now,
                    State = CheckinsStateCode.abierto.ToString(),
                };

                Tickets _ticket = new Tickets
                {
                    Parking_id = 1,
                    Codebar = BarcodeHelper.GenerateUniqueCodebar(),
                    Release_date = DateTime.Now
                };

                Console.WriteLine(existingVehicle.License_plate);
                

                _parkingService.RegisterCheckin(existingVehicle.Id, _checkin, _ticket);

                changeVehicleStateToActive(existingVehicle.License_plate);
            }

            // === Mensaje y acciones finales ===
            showTemporarySuccesMessage(labelMessageError, messageVehicleSuccesfullyRegistered, 3000);
            printTicket();
            focusScanner();
        }


        // ==================== SAVE AND GENERATE BILL ====================

        private void buttonGenerateBill_Click(object sender, EventArgs e)
        {
            insertBill();

            hideElapsedTime();
            hideCost();
            hideButtonGenerateInvoice();
            hideButtonInvoicePaid();

            printBill(true);

        }

        private void buttonPayed_Click(object sender, EventArgs e)
        {
            insertBill();

            hideElapsedTime();
            hideCost();
            hideButtonGenerateInvoice();
            hideButtonInvoicePaid();

            printBill(false);
        }

        private void insertBill()
        {
            ParkingService _parkingService = new ParkingService();
            TicketsService _ticketService = new TicketsService();
            BillsService _billService = new BillsService();

            PrintData _printData = _ticketService.getPrintData(_currentTicketId);

            String licensePlate = _printData.VehicleInfo;

            
            Bills bills = new Bills
            {
                Checkin_id = _printData.CheckinId,
                Parking_id = 1,
                Total_pay = int.Parse(labelCostValue.Text),
                Release_date = convertElapsedValueToTotalMinutes(_printData.EntryTime),
                Checkin_Time = _printData.EntryTime
            };

            changeCheckinStateToFacturado(_printData.CheckinId);
            changeVehicleStateToInactive(licensePlate);
            _billService.createBill(bills);
        }

        // ==================== UI HELPERS ====================
        private void showTextBox1() => textBox1.Visible = true;
        private void showButtonSaveVehicle() => buttonSaveVehicle.Visible = true;
        private void setTexBox1MaxLength(int limit) => textBox1.MaxLength = limit;
        private void clearTextBox1() => textBox1.Clear();
        private void setValueToLabel6(String txt) => label6.Text = txt;
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

        private void FormHome_Load(object sender, EventArgs e)
        {

        }

        private void loadParkingData()
        {
            InfoParkingService _infoParkingService = new InfoParkingService();
            List<InfoParking> data = _infoParkingService.getAllInfoParking();
            labelTitleParking.Text = data[0].Name_parking;
        }

        private void printTicket()
        {
            TicketsService _ticketService = new TicketsService();
            int ticketId = _ticketService.getLastIndex();
            PrintData _printData = _ticketService.getPrintData(ticketId);

            if (_printData != null)
            {
                PrintHelper.printTicket(_printData);
                showTemporarySuccesMessage(labelMessageError, "IMPRIMIENDO TICKET", 3000);
            }
            else
            {
                showTemporaryMessage(labelMessageError, "ALGO FUE MAL AL GENERAR EL TICKET", 300);
            }
        }

        private void printBill(bool shouldPrint)
        {
            BillsService _billService = new BillsService();
            int billId = _billService.getLastIndex();
            PrintData _printData = _billService.getPrintData(billId);

            if (_printData != null)
            {
                if (shouldPrint)
                {
                    PrintHelper.printInvoice(_printData);
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

        private String valueToSaveUsingVehicleWithMotor(bool vehicleWithMotor)
        {
            if (!vehicleWithMotor) return null;
            string text = textBox1.Text;
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private String valueToSaveUsingVehicleWithoutMotor(bool vehicleWithMotor)
        {
            if (vehicleWithMotor) return null;
            string text = textBox1.Text;
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private async void showTemporaryMessage(Label label, String message, int millisencods)
        {
            label.Text = message;
            label.Visible = true;
            label.ForeColor = Color.Red;
            label.BackColor = Color.Black;
            await Task.Delay(millisencods);
            label.Visible = false;
        }

        private async void showTemporarySuccesMessage(Label label, String message, int millisencods)
        {
            label.Text = message;
            label.Visible = true;
            label.ForeColor = Color.Blue;
            label.BackColor = Color.White;
            await Task.Delay(millisencods);
            label.Visible = false;
        }

        private void showElapsedTime(int value)
        {
            label4.Visible = true;
            labelElapsedValue.Visible = true;
            labelElapsedValue.Text = $"{value / 60}h {value % 60}m" ;
        }

        private void showCost(String value)
        {
            label7.Visible = true;
            labelCostValue.Visible = true;
            labelCostValue.Text = value;
        }

        private void showButtonGenerateInvoice()
        {
            buttonGenerateBill.Visible = true;
        }
        private void showButtonInvoicePaid()
        {
            buttonPayed.Visible = true;
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

        private void hideButtonGenerateInvoice()
        {
            buttonGenerateBill.Visible = false;
        }
        private void hideButtonInvoicePaid()
        {
            buttonPayed.Visible = false;
        }

        private void hideTextBoxScanner()
        {
            textBoxScanner.Size = new Size(1, 1);                 // very small
            textBoxScanner.Location = new Point(-100, -100);      // outside the form
            textBoxScanner.TabStop = false;                       // not included in Tab navigation
            textBoxScanner.BorderStyle = BorderStyle.None;        // no borders
            textBoxScanner.KeyPress += textBoxScanner_KeyPress;   // attach scanner event
        }

        private void textBoxScanner_GotFocus(object sender, EventArgs e)
        {
            HideCaret(textBoxScanner.Handle);
        }

        private void textBoxScanner_TextChanged(object sender, EventArgs e)
        {
            HideCaret(textBoxScanner.Handle);
        }

        private void changeVehicleStateToInactive(String licensePlate)
        {
            VehiclesService _vehiclesService = new VehiclesService();

            Vehicles _vehicles = new Vehicles
            {
                License_plate = licensePlate,
                State = VehicleStateCode.inactivo.ToString()
            };

            _vehiclesService.setVehicleState(_vehicles);
        }

        private void changeVehicleStateToActive(String licensePlate)
        {
            VehiclesService _vehiclesService = new VehiclesService();

            Vehicles _vehicles = new Vehicles
            {
                License_plate = licensePlate,
                State = VehicleStateCode.activo.ToString()
            };

            _vehiclesService.setVehicleStateWhatever(_vehicles);
        }

        private void changeCheckinStateToFacturado(int checkinId)
        {
            CheckinsService _checkinService = new CheckinsService();

            Checkins _chekin = new Checkins
            {
                Id = checkinId,
                State = CheckinsStateCode.facturado.ToString()
            };

            _checkinService.setCheckinState(_chekin);
        }

        private DateTime convertElapsedValueToTotalMinutes(DateTime entryTime)
        {
            String elapsedText = labelElapsedValue.Text;

            //Split by 'h' and 'm'
            int hours = int.Parse(elapsedText.Split('h')[0].Trim());
            int minutes = int.Parse(elapsedText.Split('h')[1].Replace("m", "").Trim());

            int totalMinutes = (hours * 60) + minutes;

            return entryTime.AddMinutes(totalMinutes);
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            int selStart = textBox2.SelectionStart;
            textBox2.Text = textBox2.Text.ToUpper();
            textBox2.SelectionStart = selStart;
        }
        

        private void buttonValidateOwner_Click(object sender, EventArgs e)
        {
            TicketsService _ticketService = new TicketsService();
            VehiclesTypesService _vehicleType = new VehiclesTypesService();
            VehiclesService _vehiclesService = new VehiclesService();
            ParkingService _parkingService = new ParkingService();

            string input = textBox2.Text.Trim();
            int ticketId = -1;

            if (string.IsNullOrEmpty(input))
            {
                showTemporaryMessage(labelMessageError, "POR FAVOR, INGRESAR UNA IDENTIFICACION DE VEHICULO VALIDA", 3000);
                return;
            }

            if (input.All(char.IsDigit))
            {
                ticketId = _ticketService.getIdByOwnerId(input);
            }
            else
            {
                ticketId = _ticketService.getIdByLicensePlate(input);
            }

            _currentTicketId = ticketId;  // save last input number 

            var _printData = _ticketService.getPrintData(ticketId);

            if (_printData != null)
            {
                bool vehicleStateActive = _vehiclesService.isVehicleStateActive(_printData.LicensePlate);

                if (_printData.CheckinState == "facturado")
                {
                    showTemporaryMessage(labelMessageError, "ESTE TICKET YA FUE FACTURADO", 3000);
                    hideElapsedTime();
                    hideCost();
                    hideButtonGenerateInvoice();
                    hideButtonInvoicePaid();
                    return;
                }

                if (vehicleStateActive == false && _printData.VehicleType != VehicleTypeCode.Bike.ToString())
                {
                    showTemporaryMessage(labelMessageError, "EL VEHICULO NO ESTA REGISTRADO", 3000);
                    hideElapsedTime();
                    hideCost();
                    hideButtonGenerateInvoice();
                    hideButtonInvoicePaid();
                    return;
                }

                showElapsedTime(_printData.MinutesElapsed);
                showCost(_printData.TotalPayGenerated.ToString());
                showButtonGenerateInvoice();
                showButtonInvoicePaid();
            }
            else
            {
                showTemporaryMessage(labelMessageError, "VEHICULO NO ENCONTRADO", 3000);
                hideElapsedTime();
                hideCost();
                hideButtonGenerateInvoice();
                hideButtonInvoicePaid();
            }
        }

        private void buttonFocuScanner_Click(object sender, EventArgs e)
        {

        }

        private void textBoxScanner_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}

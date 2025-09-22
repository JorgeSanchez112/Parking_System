using Parking.Models;
using Parking.Services;
using Parking.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking.Forms
{
    public partial class FormSettings : Form
    {
        private const String INITIAL_NAME_PARKING = "Nombre parqueadero";
        private const String INITIAL_ADDRESS = "Direccion";
        private const String INITIAL_NIT = "NIT";

        private int currentIdParking;

        public FormSettings()
        {
            InitializeComponent();
            this.Shown += FormSettings_shown;
        }

        private void FormSettings_shown(Object sender, EventArgs e)
        {
            loadParkingData();
            loadVehiclesTypeData();

            var logo = LogoManager.LoadLogo();

            if (!LogoManager.isLogoPathEmpty())
            {
                pictureBox1.Image?.Dispose(); // libera la imagen anterior si existía
                pictureBox1.Image = logo;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                // Mostrar imagen por defecto o dejar vacío
                pictureBox1.Image = null;
            }


            blockInfoParkinfChanged();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void loadParkingData()
        {
            InfoParkingService _infoParkingService = new InfoParkingService();

            List<InfoParking> data = _infoParkingService.getAllInfoParking();

            currentIdParking = data[_infoParkingService.getParkingIndex()].Id;
            textBoxNameParking.Text = data[_infoParkingService.getParkingIndex()].Name_parking;
            textBoxAddress.Text = data[_infoParkingService.getParkingIndex()].Address;
            textBoxNit.Text = data[_infoParkingService.getParkingIndex()].Nit;
            textBoxInfoTicket.Text = data[_infoParkingService.getParkingIndex()].Ticket_info;
            textBoxInfoBill.Text = data[_infoParkingService.getParkingIndex()].Bill_info;
        }

        private void loadVehiclesTypeData()
        {
            VehiclesTypesService _vehiclesTypeService = new VehiclesTypesService();

            numericDownFeeCar.Value = _vehiclesTypeService.GetByCode(VehicleTypeCode.Car).Fee;
            numericDownFeeMotorbike.Value = _vehiclesTypeService.GetByCode(VehicleTypeCode.Motorbike).Fee;
            numericDownFeeBike.Value = _vehiclesTypeService.GetByCode(VehicleTypeCode.Bike).Fee;

        }

        private void updateParkingData()
        {
            InfoParkingService _infoParkingService = new InfoParkingService();

            InfoParking _infoParking = new InfoParking
            {
                Id = currentIdParking,
                Name_parking = textBoxNameParking.Text,
                Address = textBoxAddress.Text,
                Nit = textBoxNit.Text,
                Ticket_info = textBoxInfoTicket.Text,
                Bill_info = textBoxInfoBill.Text
            };


            _infoParkingService.updateInfoParking(_infoParking);

        }

        private void updateVehiclesTypeFeeValues()
        {
            VehiclesTypesService _vehiclesTypeService = new VehiclesTypesService();

            var feeInputs = new Dictionary<VehicleTypeCode, decimal>
            {
                { VehicleTypeCode.Car, numericDownFeeCar.Value },
                { VehicleTypeCode.Motorbike, numericDownFeeMotorbike.Value },
                { VehicleTypeCode.Bike, numericDownFeeBike.Value }
            };

            _vehiclesTypeService.GetByCode(VehicleTypeCode.Car).Fee = (int)numericDownFeeCar.Value;
            _vehiclesTypeService.GetByCode(VehicleTypeCode.Motorbike).Fee = (int)numericDownFeeMotorbike.Value;
            _vehiclesTypeService.GetByCode(VehicleTypeCode.Bike).Fee = (int)numericDownFeeBike.Value;


            foreach (var kv in feeInputs) 
            {
                var vt = _vehiclesTypeService.GetByCode(kv.Key);

                if(vt != null)
                {
                    vt.Fee = (int)kv.Value;
                    _vehiclesTypeService.UpdateVehicleTypeFee(vt);
                }

            }

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //ButtonUpdate
        {

            updateParkingData();
            updateVehiclesTypeFeeValues();
            this.Close();

        }

        private void blockInfoParkinfChanged()
        {
            InfoParkingService _infoParkingService = new InfoParkingService();
            List<InfoParking> data = _infoParkingService.getAllInfoParking();

            if(_infoParkingService.hasNameChanged(data[0].Name_parking, INITIAL_NAME_PARKING))
                textBoxNameParking.Enabled = false;

            if (_infoParkingService.hasAddressChanged(data[0].Address, INITIAL_ADDRESS))
                textBoxAddress.Enabled = false;

            if (_infoParkingService.hasNitChanged(data[0].Nit, INITIAL_NIT))
                textBoxNit.Enabled = false;

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            String savedPath = LogoManager.uploadAndSaveLogo();
            if (!String.IsNullOrEmpty(savedPath))
            {
                pictureBox1.Image = LogoManager.LoadLogo();
                MessageBox.Show("Logo actualizado con exito");
            }
        }
    }
}

using Parking.Data;
using Parking.Forms;
using Parking.Models;
using Parking.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Parking
{
    public partial class FormHome : Form
    {

        private VehicleTypeCode currentTypeVehicle;

        public FormHome()
        {
            InitializeComponent();
            this.Shown += FormHome_shown;

        }

        private void FormHome_shown(Object sender, EventArgs e)
        {
            loadParkingData();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ajsutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings formSettings = new FormSettings();

            formSettings.FormClosed += (s, args) => loadParkingData(); //update info show once the form settings is closed

            formSettings.ShowDialog();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            setValueToLabel6("Ingresar placa");
            showLabel6();
            clearTextBox1();
            setTexBox1MaxLength(6);
            showTextBox1();
            showButtonSaveVehicle();
            currentTypeVehicle = VehicleTypeCode.Car;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            setValueToLabel6("Ingresar placa");
            showLabel6();
            clearTextBox1();
            setTexBox1MaxLength(6);
            showTextBox1();
            showButtonSaveVehicle();
            currentTypeVehicle = VehicleTypeCode.Motorbike;

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            setValueToLabel6("Ingresar numero de identificacion");
            showLabel6();
            clearTextBox1();
            setTexBox1MaxLength(10);
            showTextBox1();
            showButtonSaveVehicle();
            currentTypeVehicle = VehicleTypeCode.Bike;

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void listaVehiculosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVehiculesHistory formVehiculesHistory = new FormVehiculesHistory();
            formVehiculesHistory.Show();
        }

        private void showTextBox1()
        {
            textBox1.Visible = true;
        }

        private void showButtonSaveVehicle()
        {
            buttonSaveVehicle.Visible = true;
        }

        private void setTexBox1MaxLength(int limit)
        {
            textBox1.MaxLength = limit;
        }

        private void clearTextBox1()
        {
            textBox1.Clear();
        }

        private void setValueToLabel6(String txt)
        {
            label6.Text = txt;
        }

        private void showLabel6()
        {
            label6.Visible = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void loadParkingData()
        {
            InfoParkingService _infoParkingService = new InfoParkingService();

            List<InfoParking> data = _infoParkingService.getAllInfoParking();

            labelTitleParking.Text = data[0].Name_parking;
        }

        private void buttonSaveVehicle_Click(object sender, EventArgs e)
        {
            ParkingService _parkingService = new ParkingService();
            VehiclesTypesService _vehiclesTypeService = new VehiclesTypesService();
            VehiclesService _vehiclesService = new VehiclesService();


            if( _vehiclesService.validateLicensePlateExist(valueToSaveAccordingToTypeVehicle(_vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle))) && _vehiclesService.isVehicleStateActive(valueToSaveAccordingToTypeVehicle(_vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle))) ){

                showTemporaryMessage(labelMessageError, $"Ya se encuentra un vehiculo registrado con la siguiente placa: {valueToSaveAccordingToTypeVehicle(_vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle))}", 3000);
            }
            else
            {
                Vehicles _vehicle = new Vehicles
                {
                    Type_id = _vehiclesTypeService.GetId(currentTypeVehicle),
                    License_plate = valueToSaveAccordingToTypeVehicle(_vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle)),
                    Owner_id = valueToSaveAccordingToTypeVehicle(_vehiclesService.hasTypeVehicleLicensePlate(currentTypeVehicle)),
                    State = VehicleStateCode.activo.ToString()
                };

                Checkins _checkin = new Checkins
                {
                    Vehicle_id = -1,
                    EntryTime = DateTime.Now,
                    State = CheckinsStateCode.abierto.ToString(),
                };

                Tickets _ticket = new Tickets
                {
                    Checkin_id = -0,
                    Parking_id = 1,
                    Codebar = "falta metodo",
                    Release_date = DateTime.Now
                };

                _parkingService.RegisterVehicleCheckin(_vehicle, _checkin, _ticket);
            }
        }

        private void labelMessageError_Click(object sender, EventArgs e)
        {

        }

        private String valueToSaveAccordingToTypeVehicle(bool vehicleWithMotor)
        {
            String result = "NO";

            if (vehicleWithMotor)
                result = textBox1.Text;

            return result.Equals("No") ? null : result;
        }


        private async void showTemporaryMessage(Label label, String message, int millisencods)
        {
            label.Text = message;
            label.Visible = true;

            await Task.Delay(millisencods);

            label.Visible = false;

        }
    }
}

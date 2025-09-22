using Parking.Data;
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

namespace Parking.Forms
{
    public partial class FormVehiculesHistory : Form
    {
        public FormVehiculesHistory()
        {
            InitializeComponent();
            this.Shown += FormVehiculesHistory_shown;
        }

        private void FormVehiculesHistory_shown(Object sender, EventArgs e)
        {
            loadDataToDataGridView();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadDataToDataGridView()
        {
            CheckinsService _checkinsService = new CheckinsService();
            VehiclesTypesService _vehicleTypesService = new VehiclesTypesService();
            ParkingService _parkingService = new ParkingService();

            var checkins = _checkinsService.getAllCheckinsData()
                .Where(c => c.State == "abierto") // only active/open checkins
                .ToList();

            var bindingList = checkins.Select(c =>
            {
                int elapsedMinutes = _parkingService.getElapsedMinutes(DateTime.Now,c.EntryTime);
                    return new
                    {
                        VehicleTypeName = _vehicleTypesService.GetVehicleTypeSpanish(c.VehicleType?.Name),
                        VehicleInformation = !String.IsNullOrEmpty(c.Vehicle?.License_plate) ? c.Vehicle.License_plate : (!String.IsNullOrEmpty(c.Vehicle?.Owner_id) ? c.Vehicle.Owner_id : "N/A"), //First validate which information show and then translate resul into spanish
                        VehicleEntryTime = c.EntryTime,
                        ElapsedTime = $"{elapsedMinutes / 60}h {elapsedMinutes % 60}m",
                        Cost = _parkingService.calculateParkingCost(elapsedMinutes, c.VehicleType?.Fee ?? 0, c.Vehicle)
                    };
            }).ToList();

            dataGridView1.Invoke(new Action(() =>
            {
                dataGridView1.DataSource = bindingList;

                dataGridView1.Columns["VehicleTypeName"].HeaderText = "Tipo de vehiculo";
                dataGridView1.Columns["VehicleInformation"].HeaderText = "Informacion del vehiculo";
                dataGridView1.Columns["VehicleEntryTime"].HeaderText = "Hora de entrada del vehiculo";
                dataGridView1.Columns["ElapsedTime"].HeaderText = "Tiempo transcurrido";
                dataGridView1.Columns["Cost"].HeaderText = "Costo";
            }));


        }

        private async void buttonUpdateVehiclesHistory_Click(object sender, EventArgs e)
        {
            buttonUpdateVehiclesHistory.Enabled = false;
            labelMessage.Visible = true;
            labelMessage.ForeColor = Color.Black;
            labelMessage.Text = "Cargando...";
            
            await Task.Run(() => loadDataToDataGridView());
            
            labelMessage.Text = "";
            buttonUpdateVehiclesHistory.Enabled = true;
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }
    }
}

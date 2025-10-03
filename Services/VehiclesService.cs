using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking.Services
{
    public class VehiclesService
    {
        private readonly VehiclesRepository _vehiclesRepository = new VehiclesRepository();



        public List<Vehicles> getAllVehiclesData() => _vehiclesRepository.GetAll();

        public bool hasTypeVehicleLicensePlate(VehicleTypeCode _vehicleTypeCode)
        {
            bool result = false;

            if(_vehicleTypeCode.Equals(VehicleTypeCode.Car) || _vehicleTypeCode.Equals(VehicleTypeCode.Motorbike))
                result = true;

            return result;
        }

        public bool validateOwnerExist(String ownerId)
        {
            return _vehiclesRepository.existsByOwnerId(ownerId);
        }

        public int getIdAccordingToLicensePlate(String licensePlate)
        {
            return _vehiclesRepository.GetIdByLicensePlate(licensePlate);
        }

        public Vehicles getByLicensePlate(String licensePlate)
        {
        return _vehiclesRepository.GetByLicensePlate(licensePlate);
        }

        public Vehicles getByOwnerId(String ownerId)
        {
            return _vehiclesRepository.GetByOwnerId(ownerId);
        }

        public bool validateLicensePlateExist(String plate)
        {
            if (plate == null)
                return false;

            return _vehiclesRepository.existsByPlate(plate);
        }

        public bool isVehicleStateActive(String licensePlate)
        {

            return _vehiclesRepository.GetStateByLicensePlate(licensePlate).Equals(VehicleStateCode.activo);
        }

        public bool IsVehicleStateActiveByOwner(string ownerId)
        {
            return _vehiclesRepository.GetStateByOwnerId(ownerId) == VehicleStateCode.activo;
        }


        public void setVehicleState(Vehicles vehicles)
        {

            string currentState = _vehiclesRepository.GetStateByLicensePlate(vehicles.License_plate).ToString();

            if (_vehiclesRepository.GetStateByLicensePlate(vehicles.License_plate).Equals(VehicleStateCode.activo))
            {
                _vehiclesRepository.UpdateStateByLicensePlate(vehicles);
                Console.WriteLine($"Estado actualizado a {vehicles.State} para placa {vehicles.License_plate}");
            }
            else
            {
                Console.WriteLine($"Estado no actualizado. Estado actual en BD: {currentState}");
            }
                

        }

        public void setVehicleStateWhatever(Vehicles vehicles)
        {

            string currentState = _vehiclesRepository.GetStateByLicensePlate(vehicles.License_plate).ToString();

            if (_vehiclesRepository.GetStateByLicensePlate(vehicles.License_plate).Equals(VehicleStateCode.inactivo))
            {
                _vehiclesRepository.UpdateStateByLicensePlate(vehicles);
                Console.WriteLine($"Estado actualizado a {vehicles.State} para placa {vehicles.License_plate}");
            }
            else
            {
                Console.WriteLine($"Estado no actualizado. Estado actual en BD: {currentState}");
            }


        }

        public bool isTextBoxLengthValid(TextBox textBox, int minLength)
        {
            String value = textBox.Text?.Trim();
            return (!String.IsNullOrEmpty(value) && value.Length >= minLength);
        }



    }
}

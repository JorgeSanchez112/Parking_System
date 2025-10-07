using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
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
            Console.WriteLine(ownerId);
            return _vehiclesRepository.GetStateByOwnerId(ownerId) == VehicleStateCode.activo;
        }


        public void setVehicleState(Vehicles vehicles)
        {

            Boolean vehicleWithMotor = validateTypeVehicleByIdentification(vehicles);


            if (vehicleWithMotor.Equals(true) && validateStateVehicle(vehicles, vehicleWithMotor))
            {
                _vehiclesRepository.UpdateStateByLicensePlate(vehicles);
            }
            else if(vehicleWithMotor.Equals(false) && validateStateVehicle(vehicles, vehicleWithMotor))
            {
                _vehiclesRepository.UpdateStateByOwnerId(vehicles);

            } else {
                Console.WriteLine($"Estado no actualizado.");
            }

        }


        private Boolean validateTypeVehicleByIdentification(Vehicles vehicles)
        {
            String regexBike = @"^\d+$";

            if (!String.IsNullOrEmpty(vehicles.Owner_id) && Regex.IsMatch(vehicles.Owner_id, regexBike))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean validateStateVehicle(Vehicles vehicles, Boolean vehicleWithMotor)
        {
            if (vehicleWithMotor.Equals(true))
            {
                return isVehicleStateActive(vehicles.License_plate);
                //Console.WriteLine($"Estado actualizado a {vehicles.State} para placa {vehicles.License_plate}");
            }
            else
            {
                return IsVehicleStateActiveByOwner(vehicles.Owner_id);
            }
        }

        public void setVehicleStateWhatever(Vehicles vehicles)
        {
            Boolean vehicleWithMotor = validateTypeVehicleByIdentification(vehicles);


            if (vehicleWithMotor.Equals(true) && !validateStateVehicle(vehicles, vehicleWithMotor))
            {
                _vehiclesRepository.UpdateStateByLicensePlate(vehicles);
            }
            else if (vehicleWithMotor.Equals(false) && !validateStateVehicle(vehicles, vehicleWithMotor))
            {
                _vehiclesRepository.UpdateStateByOwnerId(vehicles);

            }
            else
            {
                Console.WriteLine($"Estado no actualizado.");
            }


        }

        public bool isTextBoxLengthValid(TextBox textBox, int minLength)
        {
            String value = textBox.Text?.Trim();
            return (!String.IsNullOrEmpty(value) && value.Length >= minLength);
        }



    }
}

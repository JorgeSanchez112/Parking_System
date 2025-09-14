using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class VehiclesService
    {
        private readonly VehiclesRepository _vehiclesRepository = new VehiclesRepository();

        public List<Vehicles> getAllVehiclesData() => _vehiclesRepository.GetAll();

        public void updateVehicles(Vehicles vehicles)
        {
            if (vehicles.Id < 0)
                throw new Exception("El checkinId es obligatorio.");

            _vehiclesRepository.Update(vehicles);

        }

        public bool hasTypeVehicleLicensePlate(VehicleTypeCode _vehicleTypeCode)
        {
            bool result = false;

            if(_vehicleTypeCode.Equals(VehicleTypeCode.Car) || _vehicleTypeCode.Equals(VehicleTypeCode.Motorbike))
                result = true;

            return result;
        }

        public bool validateLicensePlateExist(String plate)
        {
            return _vehiclesRepository.existsByPlate(plate);
        }

        public bool isVehicleStateActive(String licensePlate)
        {
            bool result = false;

            if(_vehiclesRepository.GetStateByLicensePlate(licensePlate).Equals(VehicleStateCode.activo))
                result = true;

            return result;
        }



    }
}

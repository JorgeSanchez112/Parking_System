using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class VehiclesTypesService
    {
        private readonly VehicleTypeRepository _vehicleTypeRepository = new VehicleTypeRepository();

        private Dictionary<VehicleTypeCode, VehicleType> _vehicleTypes;

        public VehiclesTypesService()
        {
            Refresh();
        }

        /// <summary>
        /// Recarga los tipos de vehículos desde la base de datos en el diccionario.
        /// </summary>
        public void Refresh()
        {
            var data = _vehicleTypeRepository.GetAll();

            _vehicleTypes = data
                .Where(v => !string.IsNullOrEmpty(v.Name) &&
                            Enum.TryParse(v.Name, out VehicleTypeCode code))
                .ToDictionary(
                    v => (VehicleTypeCode)Enum.Parse(typeof(VehicleTypeCode), v.Name),
                    v => v
                );
        }

        /// <summary>
        /// Devuelve todos los VehicleType de la BD (lista cruda, por si la necesitas).
        /// </summary>
        public List<VehicleType> GetAllVehicleTypes() => _vehicleTypeRepository.GetAll();

        /// <summary>
        /// Obtiene un VehicleType por su código (Car, Bike, etc.).
        /// </summary>
        public VehicleType GetByCode(VehicleTypeCode code) =>
            _vehicleTypes.TryGetValue(code, out var type) ? type : null;

        /// <summary>
        /// Obtiene el Id de un tipo de vehículo.
        /// </summary>
        public int GetId(VehicleTypeCode code) =>
            _vehicleTypes.TryGetValue(code, out var type) ? type.Id : -1;

        /// <summary>
        /// Obtiene la tarifa (Fee) de un tipo de vehículo.
        /// </summary>
        public int GetFee(VehicleTypeCode code) =>
            _vehicleTypes.TryGetValue(code, out var type) ? type.Fee : 0;

        /// <summary>
        /// Actualiza la tarifa de un tipo de vehículo y refresca el cache.
        /// </summary>
        public void UpdateVehicleTypeFee(VehicleType vehicleType)
        {
            if (vehicleType.Id <= 0)
                throw new Exception("El Id del tipo de vehículo es obligatorio.");

            if (vehicleType.Fee < 0)
                throw new Exception("La tarifa no puede ser menor a 0.");

            _vehicleTypeRepository.updateFee(vehicleType);

            // Actualizar cache para reflejar cambios inmediatamente
            Refresh();
        }

    }
}

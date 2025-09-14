using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class InfoParkingService
    {

        private readonly InfoParkingRepository _infoParkingRepository = new InfoParkingRepository();

        private const int PARKING_ID = 0;

        public void createInfoParking(InfoParking infoParking)
        {
            if (infoParking.Id < 0)
                throw new Exception("El checkinId es obligatorio.");

            if (String.IsNullOrWhiteSpace(infoParking.Name_parking) )
                throw new Exception("El nombre del parqueadero es obligatorio");

            if (String.IsNullOrWhiteSpace(infoParking.Address))
                throw new Exception("La direccion del establecimiento es obligatoria");

            if (String.IsNullOrWhiteSpace(infoParking.Ticket_info))
                throw new Exception("Informacion del ticket es obligatorio");

            if (String.IsNullOrWhiteSpace(infoParking.Bill_info))
                infoParking.Bill_info = "Esperamos haya tenido un buen servicio";

            _infoParkingRepository.insert(infoParking);
        }

        public List<InfoParking> getAllInfoParking() => _infoParkingRepository.GetAll();

        public void updateInfoParking(InfoParking infoParking)
        {
            if (infoParking.Id < 0)
                throw new Exception("El checkinId es obligatorio.");

            if (String.IsNullOrWhiteSpace(infoParking.Name_parking))
                throw new Exception("El nombre del parqueadero es obligatorio");

            if (String.IsNullOrWhiteSpace(infoParking.Address))
                throw new Exception("direccion");

            _infoParkingRepository.update(infoParking);

        }
        
        public bool hasNameChanged(string currentName, string initialName)
            => !string.Equals(currentName?.Trim(), initialName?.Trim(), StringComparison.Ordinal);

        public bool hasAddressChanged(string currentAddress, string initialAddress)
            => !string.Equals(currentAddress?.Trim(), initialAddress?.Trim(), StringComparison.Ordinal);
        
        public bool hasNitChanged(string currentNit, string initialNit)
            => !string.Equals(currentNit?.Trim(), initialNit?.Trim(), StringComparison.Ordinal);

        public int getParkingIndex()
        {
            return PARKING_ID;
        }

    }
}

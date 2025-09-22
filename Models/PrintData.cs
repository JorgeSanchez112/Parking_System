using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Models
{
    public class PrintData
    {

        private int _id;
        private String _parkingName;
        private String _parkingAddress;
        private String _parkingNit;
        private String _ticketInfo;
        private String _billInfo;

        private String _vehicleType;
        private String _licensePlate;
        private String _ownerId;
        private String _vehicleInfo;

        private int _checkinId;
        private String _checkinState;
        private DateTime _entryTime;
        private DateTime _exitTime;

        private String _ticketCode;
        private int _feePerMinute; // tarifa base

        private int _totalPay;

        // Propiedades públicas
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public String ParkingName
        {
            get => _parkingName;
            set => _parkingName = value;
        }

        public String ParkingAddress
        {
            get => _parkingAddress;
            set => _parkingAddress = value;
        }

        public String ParkingNit
        {
            get => _parkingNit;
            set => _parkingNit = value;
        }

        public String TicketInfo
        {
            get => _ticketInfo;
            set => _ticketInfo = value;
        }

        public String BillInfo
        {
            get => _billInfo;
            set => _billInfo = value;
        }

        public String VehicleType
        {
            get => _vehicleType;
            set => _vehicleType = value;
        }

        public String LicensePlate
        {
            get => _licensePlate;
            set => _licensePlate = value;
        }

        public String OwnerId
        {
            get => _ownerId;
            set => _ownerId = value;
        }

        public String VehicleInfo
        {
            get => _vehicleInfo;
            set => _vehicleInfo = value;
        }

        public int CheckinId
        {
            get => _checkinId;
            set => _checkinId = value;
        }

        public String CheckinState
        {
            get => _checkinState;
            set => _checkinState = value;
        }
        public DateTime EntryTime
        {
            get => _entryTime;
            set => _entryTime = value;
        }

        public DateTime ExitTime
        {
            get => _exitTime;
            set => _exitTime = value;
        }

        public String TicketCode
        {
            get => _ticketCode;
            set => _ticketCode = value;
        }

        public int FeePerMinute
        {
            get => _feePerMinute;
            set => _feePerMinute = value;
        }

        public int TotalPay
        {
            get => _totalPay;
            set => _totalPay = value;
        }

        // Tiempo en minutos
        public int MinutesElapsed
        {
            get
            {
                var endTime = (_exitTime == default) ? DateTime.Now : _exitTime;
                if (_entryTime == default) return 0;

                return (int)Math.Ceiling((endTime - _entryTime).TotalMinutes);
            }
        }

        // Tiempo total (TimeSpan)
        public TimeSpan TotalTime
        {
            get
            {
                var endTime = (_exitTime == default) ? DateTime.Now : _exitTime;
                if (_entryTime == default) return TimeSpan.Zero;

                return endTime - _entryTime;
            }
        }

        // Pago total calculado
        public int TotalPayGenerated
        {
            get
            {
                return MinutesElapsed * _feePerMinute;
            }
        }

    }
}

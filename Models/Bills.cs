using System;

namespace Parking.Models
{
    public class Bills
    {
        private int _id;
        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        private int _checkin_id;
        public int Checkin_id
        {
            get => _checkin_id;
            set => _checkin_id = value;
        }

        private int _parking_id;
        public int Parking_id
        {
            get => _parking_id;
            set => _parking_id = value;
        }

        private int _total_pay;
        public int Total_pay
        {
            get => _total_pay;
            set => _total_pay = value;
        }

        private DateTime _releaseDate;
        public DateTime Release_date
        {
            get => _releaseDate;
            set => _releaseDate = value;
        }

        private DateTime _checkin_time;
        public DateTime Checkin_Time
        {
            get => _checkin_time;
            set => _checkin_time = value;
        }
    }
}

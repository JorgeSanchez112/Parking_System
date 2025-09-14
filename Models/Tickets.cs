using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Models
{
    public class Tickets
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

        private String _codebar;
        public String Codebar 
        { 
            get => _codebar;
            set => _codebar = value?.Trim();
        }

        private DateTime _release_date;
        public DateTime Release_date 
        {
            get => _release_date;
            set => _release_date = value;
        }
    }
}

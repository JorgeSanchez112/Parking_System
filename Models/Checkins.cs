using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Parking.Models
{
    public class Checkins
    { 
        private int _id;
        public int Id 
        {  
            get => _id;
            set => _id = value;
        }

        private int _vehicle_id;
        public int Vehicle_id
        {
            get => _vehicle_id;
            set => _vehicle_id = value;
        }

        private DateTime _entry_time;
        public DateTime EntryTime
        {
            get => _entry_time;
            set => _entry_time = value;
        }

        private String _state;
        public String State
        {
            get => _state;
            set => _state = value?.Trim();
        }

    }
}

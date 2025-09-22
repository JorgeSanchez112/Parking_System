using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace Parking.Models
{
    public class Vehicles
    {
        private int _id;
        public int Id 
        {
            get => _id;
            set => _id = value;
        }

        private String _license_plate;
        public String License_plate
        {
            get => _license_plate;
            set => _license_plate = value?.Trim().ToUpper();
        }

        private String _owner_id;
        public String Owner_id
        {
            get => _owner_id;
            set => _owner_id = value?.Trim();
        }

        private int _type_id;
        public int Type_id
        {
            get => _type_id;
            set => _type_id = value;
        }

        private String _state;
        public String State
        {
            get => _state;
            set => _state = value?.Trim();
        }
    }
}

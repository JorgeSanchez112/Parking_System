using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Parking.Models
{
    public class VehicleType
    {
        private int _id;
        public int Id 
        { 
            get => _id;
            set => _id = value;
        }

        private String _name;
        public String Name 
        { 
            get => _name;
            set => _name = value?.Trim();
        }

        private int _fee;
        public int Fee
        {
            get => _fee;
            set => _fee = value;
        }


        public VehicleTypeCode TypeCode
        {
            get 
            {
                if (!string.IsNullOrWhiteSpace(Name) && Enum.TryParse(Name, out VehicleTypeCode result))
                    return result;

                return VehicleTypeCode.Car;
            }
            set => Name = value.ToString();
        }
    }
}

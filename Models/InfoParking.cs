using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Parking.Models
{
    public class InfoParking
    {
        private int _id;   
        public int Id 
        {
            get => _id;
            set => _id = value; 
        }

        private String _name_parking;
        public String Name_parking 
        { 
            get => _name_parking; 
            set => _name_parking = value?.Trim(); 
        }
        
        private String _address;
        public String Address
        {
            get => _address;
            set => _address = value?.Trim();
        }

        private String _nit;
        public String Nit
        {
            get => _nit;
            set => _nit = value?.Trim();
        }

        private String _bill_info;
        public String Bill_info
        {
            get => _bill_info;
            set => _bill_info = value?.Trim();
        }

        private String _ticket_info;
        public String Ticket_info
        {
            get => _ticket_info;
            set => _ticket_info = value?.Trim();
        }

    }
}

using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public class InfoParkingRepository
    {

        public void insert(InfoParking infoParking)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"INSERT INTO info_parking (Name_parking, Address, Nit, Bill_info, Ticket_info) VALUES (@name, @address, @nit, @bill, @ticket)";
                cmd.Parameters.AddWithValue("@name", infoParking.Name_parking);
                cmd.Parameters.AddWithValue ("@address", infoParking.Address);
                cmd.Parameters.AddWithValue ("@nit", infoParking.Nit);
                cmd.Parameters.AddWithValue("@bill", infoParking.Bill_info);
                cmd.Parameters.AddWithValue("@ticket", infoParking.Ticket_info);

                cmd.ExecuteNonQuery();
            }
        }

        public List<InfoParking> GetAll()
        {
            var list = new List<InfoParking>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Name_parking, Address, Nit, Bill_info, Ticket_info  FROM info_parking";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new InfoParking
                        {
                            Id = reader.GetInt32(0),
                            Name_parking = reader.GetString(1),
                            Address = reader.GetString(2),
                            Nit = reader.GetString(3),
                            Bill_info = reader.GetString(4),
                            Ticket_info = reader.GetString(5)
                        });
                    }

                }
            }

            return list;
        }

        public void update(InfoParking infoParking)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"UPDATE info_parking SET Name_parking=@name, Address=@address, Nit=@nit, Bill_info=@billInfo, Ticket_info=@ticketInfo WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", infoParking.Id);
                cmd.Parameters.AddWithValue("@name", infoParking.Name_parking);
                cmd.Parameters.AddWithValue("@address", infoParking.Address);
                cmd.Parameters.AddWithValue("@nit", infoParking.Nit);
                cmd.Parameters.AddWithValue("@billInfo", infoParking.Bill_info);
                cmd.Parameters.AddWithValue("@ticketInfo", infoParking.Ticket_info);
                cmd.ExecuteNonQuery();
            }

           
        }


    }
}

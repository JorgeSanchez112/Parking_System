using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public class VehicleTypeRepository
    {
        public void insert(VehicleType vehicleType)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"INSERT INTO vehicles_types (Name, Fee) VALUES (@name, @fee)";
                cmd.Parameters.AddWithValue("@name", vehicleType.Name);
                cmd.Parameters.AddWithValue("@fee", vehicleType.Fee);
                cmd.ExecuteNonQuery();
            }
        }

        public List<VehicleType> GetAll()
        {
            var list = new List<VehicleType>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Name, Fee From vehicles_types";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new VehicleType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Fee = reader.GetInt32(2)
                        });

                    }
                }

            }

            return list;

        }

        public void updateFee(VehicleType vehicleType)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"UPDATE vehicles_types SET Fee=@fee WHERE Id=@id";

                cmd.Parameters.AddWithValue("@id", vehicleType.Id);
                cmd.Parameters.AddWithValue("@fee", vehicleType.Fee);

                cmd.ExecuteNonQuery();
            }
        }


        public void delete(VehicleType vehicleType)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"DELETE FROM vehicles_types WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", vehicleType.Id);
                cmd.ExecuteNonQuery();
            }
        }



    }
}

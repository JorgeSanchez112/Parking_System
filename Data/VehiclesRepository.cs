using Microsoft.Data.Sqlite;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public class VehiclesRepository
    {
        public long insert(SqliteConnection con, SqliteTransaction tran, Vehicles vehicles)
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;

                cmd.CommandText = @"INSERT INTO vehicles (License_plate, Owner_id, Type_id, State) VALUES (@plate, @owner, @type, @state);";

                cmd.Parameters.AddWithValue("@plate", vehicles.License_plate);
                cmd.Parameters.AddWithValue("@owner", vehicles.Owner_id);
                cmd.Parameters.AddWithValue("@type", vehicles.Type_id);
                cmd.Parameters.AddWithValue("@state", vehicles.State = "activo");

                cmd.ExecuteNonQuery();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT last_insert_rowid();";
                return (long)cmd.ExecuteScalar();
            }
        }

        public List<Vehicles> GetAll()
        {
            var list = new List<Vehicles>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, License_plate, Owner_id, Type_id, State From Vehicles";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Vehicles
                        {
                            Id = reader.GetInt32(0),
                            License_plate = reader.GetString(1),
                            Owner_id = reader.GetString(2),
                            Type_id = reader.GetInt32(3),
                            State = reader.GetString(4)
                        } );

                    }

                }
            }

            return list;
        }

        public bool existsByPlate(String plate)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"SELECT COUNT(1) FROM vehicles WHERE License_plate = @plate";

                cmd.Parameters.AddWithValue("@plate", plate);

                long count = (long)cmd.ExecuteScalar();

                return count > 0;
            }



        }

        public VehicleStateCode GetStateByLicensePlate(String plate)
        {
            var list = new List<Vehicles>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT State From Vehicles WHERE  License_plate = @plate";
                cmd.Parameters.AddWithValue("@plate", plate);

                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.Read()) 
                    {
                        String stateStr = reader.GetString(0);

                        if(Enum.TryParse<VehicleStateCode> (stateStr, true, out var stateEnum))
                            return stateEnum;
                    }

                }
            }

            return VehicleStateCode.inactivo;
        }

        public void Update(Vehicles vehicles)
        {
           using (var con = DbConnectionFactory.GetConnection())
           {
               con.Open();
               var cmd = con.CreateCommand();

               cmd.CommandText = @"UPDATE vehicles SET License_plate=@plate, Owner_id=@owner, Type_id=@type, State=@state WHERE Id=@id";
               cmd.Parameters.AddWithValue("@id", vehicles.Id);
               cmd.Parameters.AddWithValue("@plate", vehicles.License_plate);
               cmd.Parameters.AddWithValue("@owner", vehicles.Owner_id);
               cmd.Parameters.AddWithValue("@type", vehicles.Type_id);
               cmd.Parameters.AddWithValue("@state", vehicles.State);

               cmd.ExecuteNonQuery();
           }
        }

        public void Delete(Vehicles vehicles)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"DELETE FROM vehicles WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", vehicles.Id);
                cmd.ExecuteNonQuery();
            }
        }

    }
}

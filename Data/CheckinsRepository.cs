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
    public class CheckinsRepository
    {
        public long insert(SqliteConnection con, SqliteTransaction tran, Checkins checkins)
        {
            using (var cmd = con.CreateCommand())
            {

                cmd.Transaction = tran;

                cmd.CommandText = @"INSERT INTO checkins (Vehicle_id, EntryTime, State) VALUES (@vehicle_id, @entryTime, @state)";

                cmd.Parameters.AddWithValue("@vehicle_id", checkins.Vehicle_id);
                cmd.Parameters.AddWithValue("@entryTime", checkins.EntryTime);
                cmd.Parameters.AddWithValue("@state", checkins.State);

                cmd.ExecuteNonQuery();
            }

            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT last_insert_rowid();";
                return (long)cmd.ExecuteScalar();
            }
        }

        public List<Checkins> GetAll()
        {
            var list = new List<Checkins>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Vehicle_id, EntryTime, State From checkins";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Checkins
                        {
                            Id = reader.GetInt32(0),
                            Vehicle_id = reader.GetInt32(1),
                            EntryTime = reader.GetDateTime(2),
                            State = reader.GetString(3)
                        });

                    }

                }
            }

            return list;
        }

        public void Update(Checkins checkins)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"UPDATE checkins SET Vehicle_id=@vehicle_id, EntryTime=@entryTime, State=@state WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", checkins.Id);
                cmd.Parameters.AddWithValue("@vehicle_id", checkins.Vehicle_id);
                cmd.Parameters.AddWithValue("@entryTime", checkins.EntryTime);
                cmd.Parameters.AddWithValue("@state", checkins.State);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(Vehicles vehicles)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"DELETE FROM checkins WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", vehicles.Id);

                cmd.ExecuteNonQuery();
            }
        }

    }
}

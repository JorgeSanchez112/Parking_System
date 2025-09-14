using Microsoft.Data.Sqlite;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public class TicketsRepository
    {

        public void insert(SqliteConnection con, SqliteTransaction tran, Tickets tickets)
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;

                cmd.CommandText = @"INSERT INTO tickets (Checkin_id, Parking_id, Codebar, Release_date) VALUES (@checkin_id, @parking_id, @codebar, @release_date)";
                cmd.Parameters.AddWithValue("@checkin_id", tickets.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", tickets.Parking_id);
                cmd.Parameters.AddWithValue("@codebar", tickets.Codebar);
                cmd.Parameters.AddWithValue("@release_date", tickets.Release_date);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Tickets> GetAll()
        {
            var list = new List<Tickets>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Checkin_id, Parking_id, Codebar, Release_date, From tickets";

                using (var reader = cmd.ExecuteReader())
                {
                    list.Add(new Tickets
                    {
                        Id = reader.GetInt32(0),
                        Checkin_id = reader.GetInt32(1),
                        Parking_id = reader.GetInt32(2),
                        Codebar = reader.GetString(3),
                        Release_date = reader.GetDateTime(4),
                    });
                }

            }

            return list;

        }


        public void update(Tickets tickets)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"UPDATE tickets SET Checkin_id=@checkin_id, Parking_id=@parking_id, Codebar=@codebar, Release_date=@release_date WHERE Id=@id";

                cmd.Parameters.AddWithValue("@id", tickets.Id);
                cmd.Parameters.AddWithValue("@checkin_id", tickets.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", tickets.Parking_id);
                cmd.Parameters.AddWithValue("@codebar", tickets.Codebar);
                cmd.Parameters.AddWithValue("@release_date", tickets.Release_date);

                cmd.ExecuteNonQuery();
            }
        }


        public void delete(int id)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"DELETE FROM tickets WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

    }
}

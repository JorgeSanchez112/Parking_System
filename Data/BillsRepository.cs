using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public class BillsRepository
    {
        public void insert(Bills bills)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"INSERT INTO bills (Checkin_id, Parking_id, Total_pay, Release_date, Checkin_Time) VALUES (@checkin_id, @parking_id, @total_pay, @release_date, @checkin_Time)";
                cmd.Parameters.AddWithValue("@checkin_id", bills.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", bills.Parking_id);
                cmd.Parameters.AddWithValue("@total_pay", bills.Total_pay);
                cmd.Parameters.AddWithValue("@release_date", bills.Release_date);
                cmd.Parameters.AddWithValue("@checkin_Time", bills.Checkin_Time);

                cmd.ExecuteNonQuery();
            }
        }

        public List<Bills> GetAll()
        {
            var list = new List<Bills>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Checkin_id, Parking_id, Total_pay, Release_date, Checkin_Time From bills";

                using (var reader = cmd.ExecuteReader())
                {
                    list.Add(new Bills
                    {
                        Id = reader.GetInt32(0),
                        Checkin_id = reader.GetInt32(1),
                        Parking_id = reader.GetInt32(2),
                        Total_pay = reader.GetInt32(3),
                        Release_date = reader.GetDateTime(4),
                        Checkin_Time = reader.GetDateTime(5)
                    });
                }

            }

            return list;

        }

        public void update(Bills bills)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"UPDATE bills SET Checkin_id=@checkin_id, Parking_id=@parking_id, Total_pay=@total_pay, Release_date=@release_date, Checkin_time=@checkin_time WHERE Id=@id";

                cmd.Parameters.AddWithValue("@id", bills.Id);
                cmd.Parameters.AddWithValue("@checkin_id", bills.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", bills.Parking_id);
                cmd.Parameters.AddWithValue("@total_pay", bills.Total_pay);
                cmd.Parameters.AddWithValue("@release_date", bills.Release_date);
                cmd.Parameters.AddWithValue("@Checkin_time", bills.Checkin_Time);


                cmd.ExecuteNonQuery();
            }
        }


        public void delete(Bills bills)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"DELETE FROM bills WHERE Id=@id";
                cmd.Parameters.AddWithValue("@id", bills.Id);

                cmd.ExecuteNonQuery();
            }
        }

    }
}

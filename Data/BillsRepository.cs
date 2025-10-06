using Microsoft.Data.Sqlite;
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

                Console.WriteLine($"Checkin_id={bills.Checkin_id}, Parking_id={bills.Parking_id}, Total_pay={bills.Total_pay}, Release_date={bills.Release_date}, Checkin_Time={bills.Checkin_Time}");

                cmd.ExecuteNonQuery();
            }
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
                cmd.Parameters.AddWithValue("@checkin_time", bills.Checkin_Time);


                cmd.ExecuteNonQuery();
            }
        }

        public int GetLastBillId()
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT IFNULL(MAX(Id), 0) FROM bills;";

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
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

        public Bills getBillByCheckinId(int checkinId)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"
            SELECT Id, Checkin_id, Parking_id, Total_pay, Release_date, Checkin_Time
            FROM bills
            WHERE Checkin_id = @CheckinId
            LIMIT 1;
            ";

                cmd.Parameters.AddWithValue("@CheckinId", checkinId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Bills
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Checkin_id = reader.GetInt32(reader.GetOrdinal("Checkin_id")),
                            Parking_id = reader.GetInt32(reader.GetOrdinal("Parking_id")),
                            Total_pay = reader.GetInt32(reader.GetOrdinal("Total_pay")),
                            Release_date = reader.GetDateTime(reader.GetOrdinal("Release_date")),
                            Checkin_Time = reader.GetDateTime(reader.GetOrdinal("Checkin_Time"))
                        };
                    }
                }
            }

            return null;
        }

        public void insertWithTransaction(SqliteConnection con, SqliteTransaction tran, Bills bills)
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;

                cmd.CommandText = @"INSERT INTO bills (Checkin_id, Parking_id, Total_pay, Release_date, Checkin_Time) VALUES (@checkin_id, @parking_id, @total_pay, @release_date, @checkin_Time)";
                cmd.Parameters.AddWithValue("@checkin_id", bills.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", bills.Parking_id);
                cmd.Parameters.AddWithValue("@total_pay", bills.Total_pay);
                cmd.Parameters.AddWithValue("@release_date", bills.Release_date);
                cmd.Parameters.AddWithValue("@checkin_Time", bills.Checkin_Time);

                Console.WriteLine($"Checkin_id={bills.Checkin_id}, Parking_id={bills.Parking_id}, Total_pay={bills.Total_pay}, Release_date={bills.Release_date}, Checkin_Time={bills.Checkin_Time}");

                cmd.ExecuteNonQuery();
            }
        }
        public List<Bills> GetAllBills()
        {
            var list = new List<Bills>();

            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();

                cmd.CommandText = @"
            SELECT 
                b.Id AS BillId,
                b.Total_pay,
                b.Release_date,
                b.Checkin_Time,
                c.Id AS CheckinId,
                c.EntryTime,
                c.State AS CheckinState,
                v.Id AS VehicleId,
                v.License_plate,
                v.Owner_id,
                v.State AS VehicleState,
                vt.Name AS VehicleTypeName,
                vt.Fee AS VehicleFee,
                p.Id AS ParkingId,
                p.Name_parking,
                p.Address,
                p.Nit
            FROM bills b
            INNER JOIN checkins c ON b.Checkin_id = c.Id
            INNER JOIN vehicles v ON c.Vehicle_id = v.Id
            INNER JOIN vehicles_types vt ON v.Type_id = vt.Id
            INNER JOIN info_parking p ON b.Parking_id = p.Id
            ORDER BY b.Release_date DESC
        ";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // --- Vehicle info ---
                        Vehicles vehicle = new Vehicles
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                            License_plate = reader.IsDBNull(reader.GetOrdinal("License_plate")) ? null : reader.GetString(reader.GetOrdinal("License_plate")),
                            Owner_id = reader.IsDBNull(reader.GetOrdinal("Owner_id")) ? null : reader.GetString(reader.GetOrdinal("Owner_id")),
                            State = reader.GetString(reader.GetOrdinal("VehicleState"))
                        };

                        // --- Vehicle type ---
                        VehicleType vehicleType = new VehicleType
                        {
                            Name = reader.GetString(reader.GetOrdinal("VehicleTypeName")),
                            Fee = reader.GetInt32(reader.GetOrdinal("VehicleFee"))
                        };

                        // --- Parking info ---
                        InfoParking infoParking = new InfoParking
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ParkingId")),
                            Name_parking = reader.GetString(reader.GetOrdinal("Name_parking")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Nit = reader.IsDBNull(reader.GetOrdinal("Nit")) ? null : reader.GetString(reader.GetOrdinal("Nit"))
                        };

                        // --- Checkin info ---
                        Checkins checkin = new Checkins
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CheckinId")),
                            EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                            State = reader.GetString(reader.GetOrdinal("CheckinState")),
                            Vehicle = vehicle,
                            VehicleType = vehicleType
                        };

                        // --- Bill info ---
                        Bills bill = new Bills
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("BillId")),
                            Total_pay = reader.GetInt32(reader.GetOrdinal("Total_pay")),
                            Release_date = reader.GetDateTime(reader.GetOrdinal("Release_date")),
                            Checkin_Time = reader.GetDateTime(reader.GetOrdinal("Checkin_Time")),
                            Vehicle = vehicle,
                            VehicleType = vehicleType,
                            Checkin = checkin,
                            InfoParking = infoParking
                        };

                        list.Add(bill);
                    }
                }
            }

            return list;
        }


        public PrintData getPrintDataForBill(int billId)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();

                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        ip.Name_parking,
                        ip.Address,
                        ip.Nit,
                        ip.Bill_info,
                        vt.Name AS VehicleType,
                        vt.Fee,
                        v.License_plate,
                        v.Owner_id,
                        c.EntryTime,
                        b.Release_date AS ExitTime,
                        b.Total_pay,
                        t.Codebar
                    FROM bills b
                    INNER JOIN checkins c ON b.Checkin_id = c.Id
                    INNER JOIN vehicles v ON c.Vehicle_id = v.Id
                    INNER JOIN vehicles_types vt ON v.Type_id = vt.Id
                    INNER JOIN tickets t ON c.Id = t.Checkin_id
                    INNER JOIN info_parking ip ON b.Parking_id = ip.Id
                    WHERE b.Id = @billId;
                ";

                cmd.Parameters.AddWithValue("@billId", billId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new PrintData
                        {
                            ParkingName = reader["Name_parking"]?.ToString() ?? "",
                            ParkingAddress = reader["Address"]?.ToString() ?? "",
                            ParkingNit = reader["Nit"]?.ToString() ?? "",
                            BillInfo = reader["Bill_info"]?.ToString() ?? "",

                            VehicleType = reader["VehicleType"]?.ToString() ?? "",
                            FeePerMinute = reader["Fee"] != DBNull.Value ? Convert.ToInt32(reader["Fee"]) : 0,

                            VehicleInfo = !string.IsNullOrWhiteSpace(reader["License_plate"]?.ToString())
                                        ? reader["License_plate"].ToString()
                                        : (reader["Owner_id"]?.ToString() ?? ""),


                            EntryTime = reader["EntryTime"] != DBNull.Value ? Convert.ToDateTime(reader["EntryTime"]) : DateTime.MinValue,
                            ExitTime = reader["ExitTime"] != DBNull.Value ? Convert.ToDateTime(reader["ExitTime"]) : DateTime.MinValue,
                            TotalPay = reader["Total_pay"] != DBNull.Value ? Convert.ToInt32(reader["Total_pay"]) : 0,

                            TicketCode = reader["Codebar"]?.ToString() ?? ""

                        };
                    }
                }
                return null;
            }
        }


    }
}

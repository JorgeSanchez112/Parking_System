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

                cmd.CommandText = @"INSERT INTO tickets (Checkin_id, Parking_id, Codebar) VALUES (@checkin_id, @parking_id, @codebar)";
                cmd.Parameters.AddWithValue("@checkin_id", tickets.Checkin_id);
                cmd.Parameters.AddWithValue("@parking_id", tickets.Parking_id);
                cmd.Parameters.AddWithValue("@codebar", tickets.Codebar);

                cmd.ExecuteNonQuery();
            }
        }

        public int GetLastTicketId()
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT IFNULL(MAX(Id), 0) FROM tickets;";

                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
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

        public int getIdByCodeBar(String codebar)
        {
            int ticketId = -1;// valueTrap

            using (var connection = DbConnectionFactory.GetConnection())
            {
                connection.Open();

                string query = "SELECT Id FROM Tickets WHERE Codebar = @Codebar LIMIT 1";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Codebar", codebar);

                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        ticketId = Convert.ToInt32(result);
                    }
                }
            }

            return ticketId;
        }

        public int GetActiveTicketIdByLicensePlate(string licensePlate)
        {
            using (var connection = DbConnectionFactory.GetConnection())
            {
                connection.Open();
                var query = @" SELECT t.Id FROM checkins c INNER JOIN vehicles v ON v.Id = c.Vehicle_id INNER JOIN tickets t ON t.Checkin_id = c.Id
                    WHERE v.License_plate = @LicensePlate
                    AND c.State = 'abierto'
                    LIMIT 1";   // If there are more than one, return just one

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@LicensePlate", licensePlate);
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public int GetActiveTicketIdByOwnerId(string ownerId)
        {
            using (var connection = DbConnectionFactory.GetConnection())
            {
                connection.Open();
                var query = @" SELECT t.Id FROM checkins c INNER JOIN vehicles v ON v.Id = c.Vehicle_id INNER JOIN tickets t ON t.Checkin_id = c.Id
                    WHERE v.Owner_id = @OwnerId
                    AND c.State = 'abierto'
                    LIMIT 1"; // If there are more than one, return just one

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@OwnerId", ownerId);
                    var result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public PrintData getPrintDataForTicket(int ticketId)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT 
                        ip.Name_parking,
                        ip.Address,
                        ip.Nit,
                        ip.Ticket_info,
                        vt.Name AS VehicleType,
                        vt.Fee,
                        v.License_plate,
                        v.Owner_id,
                        c.Id as Checkin_id,
                        c.EntryTime,
                        c.State as Checkin_State,
                        t.Id,
                        t.Codebar
                    FROM tickets t
                    INNER JOIN checkins c ON t.Checkin_id = c.Id
                    INNER JOIN vehicles v ON c.Vehicle_id = v.Id
                    INNER JOIN vehicles_types vt ON v.Type_id = vt.Id
                    INNER JOIN info_parking ip ON t.Parking_id = ip.Id
                    WHERE t.Id = @TicketId;
                ";

                cmd.Parameters.AddWithValue("@TicketId", ticketId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var entryTime = reader["EntryTime"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["EntryTime"])
                                        : DateTime.MinValue;

                        return new PrintData
                        {
                            ParkingName = reader["Name_parking"]?.ToString() ?? "",
                            ParkingAddress = reader["Address"]?.ToString() ?? "",
                            ParkingNit = reader["Nit"]?.ToString() ?? "",
                            TicketInfo = reader["Ticket_info"]?.ToString() ?? "",
                            CheckinId = reader["Checkin_id"] != DBNull.Value ? Convert.ToInt32(reader["Checkin_id"]) : 0,

                            VehicleType = reader["VehicleType"]?.ToString() ?? "",
                            FeePerMinute = reader["Fee"] != DBNull.Value ? Convert.ToInt32(reader["Fee"]) : 0,
                            LicensePlate = reader["License_plate"].ToString() ?? "",
                            OwnerId = reader["Owner_id"].ToString() ?? "",
                            VehicleInfo = !string.IsNullOrWhiteSpace(reader["License_plate"]?.ToString())
                                        ? reader["License_plate"].ToString()
                                        : (reader["Owner_id"]?.ToString() ?? ""),

                            EntryTime = entryTime,
                            CheckinState = reader["Checkin_State"].ToString() ?? "",
                            TicketCode = reader["Codebar"]?.ToString() ?? ""
                        };
                    }
                }
                return null;
            }
        }


    }
}

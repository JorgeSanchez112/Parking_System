using Microsoft.Data.Sqlite;
using Parking.Models;
using System.Collections.Generic;

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

                cmd.CommandText = @"
                SELECT 
                    c.Id AS CheckinId,
                    c.EntryTime,
                    c.State AS CheckinState,
                    v.Id AS VehicleId,
                    v.License_plate,
                    v.Owner_id,
                    v.State AS VehicleState,
                    vt.Name AS VehicleTypeName,
                    vt.Fee AS VehicleFee,
                    t.Id AS TicketId,
                    t.Codebar,
                    t.Release_date AS TicketReleaseDate,
                    p.Id AS ParkingId,
                    p.Name_parking,
                    p.Address,
                    p.Nit
                FROM checkins c
                INNER JOIN vehicles v ON c.Vehicle_id = v.Id
                INNER JOIN vehicles_types vt ON v.Type_id = vt.Id
                INNER JOIN tickets t ON t.Checkin_id = c.Id
                INNER JOIN info_parking p ON t.Parking_id = p.Id
                ORDER BY c.EntryTime DESC
                ";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Vehicles vehicle = new Vehicles
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                            License_plate = reader.IsDBNull(reader.GetOrdinal("License_plate")) ? null : reader.GetString(reader.GetOrdinal("License_plate")),
                            Owner_id = reader.IsDBNull(reader.GetOrdinal("Owner_id")) ? null : reader.GetString(reader.GetOrdinal("Owner_id")),
                            State = reader.GetString(reader.GetOrdinal("VehicleState")),
                        };

                        VehicleType vehicleType = new VehicleType
                        {
                            Name = reader.GetString(reader.GetOrdinal("VehicleTypeName")),
                            Fee = reader.GetInt32(reader.GetOrdinal("VehicleFee"))
                        };

                        Tickets ticket = new Tickets
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("TicketId")),
                            Codebar = reader.GetString(reader.GetOrdinal("Codebar")),
                            Release_date = reader.GetDateTime(reader.GetOrdinal("TicketReleaseDate"))
                        };

                        InfoParking infoParking = new InfoParking
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ParkingId")),
                            Name_parking = reader.GetString(reader.GetOrdinal("Name_parking")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Nit = reader.IsDBNull(reader.GetOrdinal("Nit")) ? null : reader.GetString(reader.GetOrdinal("Nit"))
                        };

                        list.Add(new Checkins
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CheckinId")),
                            EntryTime = reader.GetDateTime(reader.GetOrdinal("EntryTime")),
                            State = reader.GetString(reader.GetOrdinal("CheckinState")),
                            VehicleType = vehicleType,
                            Vehicle = vehicle,
                            Ticket = ticket,
                            InfoParking = infoParking
                        });
                    }
                }
                return list;
            }
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

        public void SetCheckinState(int checkinId, string newState)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "UPDATE checkins SET State = @state WHERE Id = @id";
                cmd.Parameters.AddWithValue("@state", newState);
                cmd.Parameters.AddWithValue("@id", checkinId);
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

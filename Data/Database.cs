using Microsoft.Data.Sqlite;
using Parking.Models;
using System;
using System.IO;
using System.Numerics;

namespace Parking.Data
{
    public class Database
    {

        private const string Db_FILE = "Parking.db";

        // Crear base de datos y tablas si no existen
        public static void Initialize()
        {

            if (!File.Exists(Db_FILE))
            {
                using (File.Create(Db_FILE)) { }
            }

            using (var con = new SqliteConnection("Data Source=" + Db_FILE + ";"))
            {
                con.Open();

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS vehicles_types
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Fee INTEGER NOT NULL
                    )");

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS info_parking
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name_parking TEXT NOT NULL,
                        Address TEXT NOT NULL,
                        Nit TEXT,
                        Bill_info TEXT,
                        Ticket_info TEXT
                    )");

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS vehicles
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Type_id INTEGER NOT NULL,
                        License_plate TEXT UNIQUE,
                        Owner_id TEXT,
                        State TEXT CHECK(State IN ('activo','inactivo')) DEFAULT 'activo',
                        FOREIGN KEY (Type_id) REFERENCES vehicles_types(Id)
                    )");

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS checkins
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Vehicle_id INTEGER NOT NULL,
                        EntryTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        State TEXT CHECK(State IN ('abierto','cerrado','facturado')) DEFAULT 'abierto',
                        FOREIGN KEY (Vehicle_id) REFERENCES vehicles(Id)
                    )");

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS tickets
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Checkin_id INTEGER NOT NULL,
                        Parking_id INTEGER NOT NULL,
                        Codebar TEXT UNIQUE,
                        FOREIGN KEY (Checkin_id) REFERENCES checkins(Id),
                        FOREIGN KEY (Parking_id) REFERENCES info_parking(Id)
                    )");

                ExecuteNonQuery(con, @"CREATE TABLE IF NOT EXISTS bills
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Checkin_id INTEGER NOT NULL,
                        Parking_id INTEGER NOT NULL,
                        Total_pay INTEGER NOT NULL,
                        Release_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        Checkin_Time DATETIME NOT NULL,
                        FOREIGN KEY (Checkin_id) REFERENCES checkins(Id),
                        FOREIGN KEY (Parking_id) REFERENCES info_parking(Id)
                    )");

                SeedData(con);
            }
        }

        private static void ExecuteNonQuery(SqliteConnection con, string sql)
        {
            using (var cmd = new SqliteCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedData(SqliteConnection con)
        { 
            // vehicles_types
            ExecuteNonQuery(con, @"INSERT OR IGNORE INTO vehicles_types (Id, Name, Fee) VALUES (1, 'Car', 0)");
            ExecuteNonQuery(con, @"INSERT OR IGNORE INTO vehicles_types (Id, Name, Fee) VALUES (2, 'Motorbike', 0)");
            ExecuteNonQuery(con, @"INSERT OR IGNORE INTO vehicles_types (Id, Name, Fee) VALUES (3, 'Bike', 0)");

            // info_parking
            ExecuteNonQuery(con, @"INSERT OR IGNORE INTO info_parking 
            (Id, Name_parking, Address, Nit, Bill_info, Ticket_info) 
            VALUES (1, 'Nombre parqueadero', 'Direccion', 'NIT', 'Nuevo', 'Nuevo')");
         }

        public static void DeleteDatabase()
        {
            if (File.Exists(Db_FILE))
            {
                File.Delete(Db_FILE);
                Console.WriteLine("Base de datos eliminada.");
            }
            else
            {
                Console.WriteLine("No existe la base de datos para eliminar.");
            }
        }

        //public static void showSomeData()
        //{
        //    Console.WriteLine("------------------------------------- BD -------------------------------------------");
        //    using (var con = new SqliteConnection("Data Source=" + Db_FILE + ";"))
        //    {
        //        con.Open();

        //        string query = @"
        //        SELECT v.Id,
        //               v.License_plate AS Placa,
        //               v.Owner_id      AS DocumentoPropietario,
        //               vt.Name         AS TipoVehiculo,
        //               v.State         AS Estado
        //        FROM vehicles v
        //        JOIN vehicles_types vt ON v.Type_id = vt.Id;";

        //        using (var cmd = new SqliteCommand(query, con))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                int id = reader.GetInt32(0);

        //                string placa = reader.IsDBNull(1) ? "(sin placa)" : reader.GetString(1);
        //                string owner = reader.IsDBNull(2) ? "(sin propietario)" : reader.GetString(2);
        //                string tipo = reader.IsDBNull(3) ? "(sin tipo)" : reader.GetString(3);
        //                string estado = reader.IsDBNull(4) ? "(sin estado)" : reader.GetString(4);

        //                Console.WriteLine($"Id: {id}, Placa: {placa}, Propietario: {owner}, Tipo: {tipo}, Estado: {estado}");
        //            }
        //        }
        //    }
        //}


    }
}

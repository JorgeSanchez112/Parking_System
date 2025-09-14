using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Data
{
    public static class DbConnectionFactory
    {
        private const String ConnectionString = "Data Source=Parking.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

    }
}

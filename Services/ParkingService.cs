using Microsoft.Data.Sqlite;
using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class ParkingService
    {

        private readonly VehiclesRepository vehiclesRepo = new VehiclesRepository();
        private readonly CheckinsRepository checkinsRepo = new CheckinsRepository();
        private readonly TicketsRepository ticketsRepo = new TicketsRepository();
        private readonly BillsRepository billsRepository = new BillsRepository();

        private readonly PrintData printData = new PrintData();

        public void RegisterVehicleCheckin(Vehicles vehicles, Checkins checkins, Tickets tickets)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert vehícle
                        long vehicleId = vehiclesRepo.insert(con, tran, vehicles);

                        checkins.Vehicle_id = (int)vehicleId;
                        if(checkins.Vehicle_id < 0 )
                            throw new Exception("El vehicle id no puede ser menor a 0.");

                        // 2. Insert checkin
                        long checkinId = checkinsRepo.insert(con, tran, checkins);

                        tickets.Checkin_id = (int)checkinId;
                        if (tickets.Checkin_id < 0)
                            throw new Exception("El checkin id no puede ser menor a 0.");
                        
                        // 3. Insert ticket
                        ticketsRepo.insert(con, tran, tickets);

                        tran.Commit(); // Everything OK
                    }
                    catch
                    {
                        tran.Rollback(); // If something fail, everything is undone
                        throw;
                    }
                }
            }
        }

        public void RegisterCheckin(int idVehicle , Checkins checkins, Tickets tickets)
        {
            using (var con = DbConnectionFactory.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {

                        checkins.Vehicle_id = idVehicle;
                        if (checkins.Vehicle_id < 0)
                            throw new Exception("El vehicle id no puede ser menor a 0.");

                        // 1. Insert checkin
                        long checkinId = checkinsRepo.insert(con, tran, checkins);

                        tickets.Checkin_id = (int)checkinId;
                        if (tickets.Checkin_id < 0)
                            throw new Exception("El checkin id no puede ser menor a 0.");

                        // 2. Insert ticket
                        ticketsRepo.insert(con, tran, tickets);

                        tran.Commit(); // Everything OK
                    }
                    catch
                    {
                        tran.Rollback(); // If something fail, everything is undone
                        throw;
                    }
                }
            }
        }

        public int calculateParkingCost(int elapsedMinutes, int vehicleFee, Vehicles vehicle)
        {

            if(vehicle != null)
            {

                return (elapsedMinutes * vehicleFee);
            }

            return 0;
            
        }

        public int getElapsedMinutes(DateTime currentTime, DateTime entryTime)
        {
            return (int)Math.Ceiling((currentTime - entryTime).TotalMinutes);
        }

    }
}

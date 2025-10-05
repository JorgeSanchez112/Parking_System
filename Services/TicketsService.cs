using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;

namespace Parking
{
    public class TicketsService
    {
        private readonly TicketsRepository _ticketsRepository = new TicketsRepository();

        public List<Tickets> getAllTickets() => _ticketsRepository.GetAll();

        public void deleteTickect(int id)
        {
            if (id <= 0)
                throw new Exception("Id invalido");

            _ticketsRepository.delete(id);
        }

        public int getIdByCodeBar(String codebar)
        {
            return _ticketsRepository.getIdByCodeBar(codebar);
        }

        public int getIdByLicensePlate(String licensePlate)
        {
            return _ticketsRepository.GetActiveTicketIdByLicensePlate(licensePlate);
        }

        public int getIdByOwnerId(String ownerId)
        {
            return _ticketsRepository.GetActiveTicketIdByOwnerId(ownerId);
        }


        public int getCheckinIdByTicketId(int ticketId)
        {
            return _ticketsRepository.getCheckinIdByTicketId(ticketId);
        }

        public PrintData getPrintData(int id)
        {
            return _ticketsRepository.getPrintDataForTicket(id);
        }

        public int getLastIndex()
        {
            return _ticketsRepository.GetLastTicketId();
        }

    }
}

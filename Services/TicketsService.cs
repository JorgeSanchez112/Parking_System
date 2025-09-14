using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class BillsService
    {
        private readonly BillsRepository _billsRepository = new BillsRepository();

        public void createBill(Bills bills)
        {
            _billsRepository.insert(bills);
        }

        public List<Bills> getAllBills() => _billsRepository.GetAllBills();

        public void updateBill(Bills bills)
        {
            if (bills.Id < 0)
                throw new Exception("El checkinId es obligatorio.");

            _billsRepository.update(bills);

        }

        public Bills getBillByCheckinId(int checkinId)
        {
            return _billsRepository.getBillByCheckinId(checkinId);
        }

        public PrintData getPrintData(int id)
        {
            return _billsRepository.getPrintDataForBill(id);
        }

        public int getLastIndex()
        {
            return _billsRepository.GetLastBillId();
        }

    }
}

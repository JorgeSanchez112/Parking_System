using Parking.Data;
using Parking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Services
{
    public class CheckinsService
    {
        private readonly CheckinsRepository _checkinsRepository = new CheckinsRepository();


        public List<Checkins> getAllCheckinsData() => _checkinsRepository.GetAll();

        public void updateCheckin(Checkins checkins)
        {
            if (checkins.Id < 0)
                throw new Exception("El checkinId es obligatorio.");

            _checkinsRepository.Update(checkins);

        }

        public void setCheckinState(Checkins _checkins)
        {
            _checkinsRepository.SetCheckinState(_checkins.Id, _checkins.State);
        }


    }
}

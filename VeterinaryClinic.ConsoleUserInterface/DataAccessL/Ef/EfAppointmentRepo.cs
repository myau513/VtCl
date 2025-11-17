using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinaryClinic.Core.Essence;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Core.DTO;

namespace DataAccessL.Ef
{
    public class EfAppointmentRepo : EfBaseRepository<AppointmentDto>
    {
        public EfAppointmentRepo(Context context) : base(context)
        {
            
        }
    }
}


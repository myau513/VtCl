using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto.Essence;
using Microsoft.EntityFrameworkCore;

namespace DataAccessL.Ef
{
    public class EfAppointmentRepo : EfBaseRepository<AppointmentDto>
    {
        public EfAppointmentRepo(Context context) : base(context)
        {
            
        }
    }
}


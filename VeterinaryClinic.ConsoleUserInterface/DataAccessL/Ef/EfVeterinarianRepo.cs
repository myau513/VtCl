using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using VeterinaryClinic.Core.Essence;
using Microsoft.EntityFrameworkCore;
using VeterinaryClinic.Core.DTO;
namespace DataAccessL.Ef
{
    public class EfVeterinarianRepo : EfBaseRepository<VeterinarianDto>
    {
        public EfVeterinarianRepo(Context context) : base(context)
        {

        }
    }
}
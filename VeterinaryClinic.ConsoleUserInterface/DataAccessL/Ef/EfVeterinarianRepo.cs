using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dto.Essence;
namespace DataAccessL.Ef
{
    public class EfVeterinarianRepo : EfBaseRepository<VeterinarianDto>
    {
        public EfVeterinarianRepo(Context context) : base(context)
        {

        }
    }
}
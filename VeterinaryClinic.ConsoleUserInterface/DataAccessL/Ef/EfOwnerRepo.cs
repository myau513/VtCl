using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dto.Essence;
namespace DataAccessL.Ef
{
    public class EfOwnerRepo : EfBaseRepository<OwnerDto>
    {
        public EfOwnerRepo(Context context) : base(context)
        {

        }
    }
}
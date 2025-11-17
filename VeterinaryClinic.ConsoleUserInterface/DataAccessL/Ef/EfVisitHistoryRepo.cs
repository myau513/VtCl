using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dto.Essence;
namespace DataAccessL.Ef
{
    public class EfVisitHistoryRepo : EfBaseRepository<VisitHistoryDto>
    {
        public EfVisitHistoryRepo(Context context) : base(context)
        {

        }
    }
}
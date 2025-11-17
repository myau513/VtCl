using DataAccessL;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Dto.Essence;
namespace DataAccessL.Ef
{
    public class EfPetRepo : EfBaseRepository<PetDto>
    {
        public EfPetRepo(Context context) : base(context)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AutolotDAL_Core2.EF;
using AutolotDAL_Core2.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;

namespace AutoLotDAL_Core2.Repos
{
    public class InventoryRepo : BaseRepo<inventory>, IInventoryRepo
    {
        public InventoryRepo(AutoLotContext context) : base(context)
        {
        }

        public override List<inventory> GetAll() 
            => GetAll(x=>x.PetName,true).ToList();

        public List<inventory> Search(String searchString)
            => Context.Cars.Where(c => Functions.Like(c.PetName, $"%{searchString}%")).ToList();

        public List<inventory> GetPinkCars() 
            => GetSome(x => x.Color == "Pink");

        public List<inventory> GetRelatedData() 
            => Context.Cars.FromSqlRaw("SELECT * FROM Inventory").Include(x => x.Orders).ThenInclude(x => x.Customer).ToList();
    }
}

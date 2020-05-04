using System;
using System.Collections.Generic;
using AutolotDAL_Core2.Models;


namespace AutoLotDAL_Core2.Repos
{
    public interface IInventoryRepo : IRepo<inventory>
    {
        List<inventory> Search(String searchString);
        List<inventory> GetPinkCars();
        List<inventory> GetRelatedData();
    }
}
using System;
using System.Linq;
using AutoLotDAL_Core2.DataInitialization;
using AutolotDAL_Core2.EF;
using AutolotDAL_Core2.Models;
using AutoLotDAL_Core2.Repos;
using Microsoft.EntityFrameworkCore;

namespace AutolotDAL_Core2.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** Fun with ADO.NET EF Core 2 *****\n");

            using (var context = new AutoLotContext())
            {
                MyDataInitializer.RecreateDatabase(context);
                MyDataInitializer.InitializeData(context);
                foreach (inventory c in context.Cars)
                {
                    Console.WriteLine(c);
                }
            }

            Console.WriteLine("***** Using a Repository *****\n");
            using (var context = new AutoLotContext())
            {
                using var repo = new InventoryRepo(context);
                foreach (inventory c in repo.GetAll())
                {
                    Console.WriteLine(c);
                }
            }

            TestConcurrency();
            Console.ReadLine();
        }

        private static void AddNewRecord(inventory car)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                repo.Add(car);
            }
        }

        private static void UpdateRecord(int carId)
        {
            using (var repo = new InventoryRepo(new AutoLotContext()))
            {
                // Grab the car, change it, save! 
                var carToUpdate = repo.GetOne(carId);
                if (carToUpdate == null) return;
                carToUpdate.Color = "Blue";
                repo.Update(carToUpdate);
            }
        }

        private static void RemoveRecordByCar(inventory carToDelete)
        {
            using (var context = new AutoLotContext())
            {
                using var repo = new InventoryRepo(context);
                repo.Delete(carToDelete);
            }
        }

        private static void RemoveRecordById(int carId, byte[] timeStamp)
        {
            using (var context = new AutoLotContext())
            {
                using var repo = new InventoryRepo(context);
                repo.Delete(carId, timeStamp);
            }
        }

        private static void TestConcurrency()
        {
            var repo1 = new InventoryRepo(new AutoLotContext());
            //Use a second repo to make sure using a different context
            var repo2 = new InventoryRepo(new AutoLotContext());
            var car1 = repo1.GetOne(1);
            var car2 = repo2.GetOne(1);
            car1.PetName = "NewName";
            repo1.Update(car1);
            car2.PetName = "OtherName";
            try
            {
                repo2.Update(car2);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var currentValues = entry.CurrentValues;
                var originalValues = entry.OriginalValues;
                var dbValues = entry.GetDatabaseValues();
                Console.WriteLine(" ******** Concurrency ************");
                Console.WriteLine("Type\tPetName");
                Console.WriteLine($"Current:\t{currentValues[nameof(inventory.PetName)]}");
                Console.WriteLine($"Orig:\t{originalValues[nameof(inventory.PetName)]}");
                Console.WriteLine($"db:\t{dbValues[nameof(inventory.PetName)]}");
            }
        }
    }
}
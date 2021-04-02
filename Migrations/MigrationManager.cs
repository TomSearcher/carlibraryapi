using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using carlibraryapi.Model;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carlibraryapi.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<CarLibraryContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();

                        CarBrand b = new CarBrand { Name = "Porsche"};
                        CarModel c = new CarModel { Name = "911", CarBrand=b};
                        int bC = appContext.CarBrands.Count();
                        int cC = appContext.CarModels.Count();
                        if (bC+cC == 0){
                            appContext.CarBrands.Add(b);
                            appContext.CarModels.Add(c);
                            appContext.SaveChanges();
                        }
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything you think it's needed
                        throw;
                    }
                }
            }
            return host;
        }        
    }
}
using RMarket.ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMarket.ClassLib
{
    public class Bootstrapper
    {
        public void Inicialize()
        {

        }

        public void SetMigrations<TConfiguration>()
            where TConfiguration: DbMigrationsConfiguration<RMarket.ClassLib.Entities.RMarketContext>, new()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RMarketContext, TConfiguration>());
        }
    }
}

namespace lab_3_EntityFramework_ProductionSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<lab_3_EntityFramework_ProductionSystem.MainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "lab_3_EntityFramework_ProductionSystem.MainDbContext";
        }

        protected override void Seed(lab_3_EntityFramework_ProductionSystem.MainDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

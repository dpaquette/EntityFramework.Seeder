EntityFramework.Seeder
==========================

A collection of helper methods to help seed entity framework databases. When seeding your database with lookup entities, we often currently do the following in our Seed methods:

  context.Countries.AddOrUpdate(c => c.Code, new Country[]
            {
                new Country{ Code = "CA", Name = "Canada"},
                new Country{ Code = "USA", Name = "United States"}
                //etc...
            });

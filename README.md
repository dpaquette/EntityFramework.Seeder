EntityFramework.Seeder
==========================

A collection of helper methods to help seed entity framework databases. When seeding your database with lookup entities, we often currently do the following in our Seed methods:

    context.Countries.AddOrUpdate(c => c.Code, new Country[]
            {
                new Country{ Code = "CA", Name = "Canada"},
                new Country{ Code = "USA", Name = "United States"}
                //etc...
            });

While this is managable for small amounts of data, our seed methods can become difficult to manage in many real world scenarios.

Using EntiyFramework.Seeder, you can seed your database from CSV files. 

First, create a CSV file with column names matching the properties of your entity. In the case of Country, the file would look as follows

    Code, Name
    CA, Canada
    USA, United States
    etc...
    

Next, include the file in project that contains your Seed method and change the Build Action to Embedded Resource.

Finally, call the SeedFromResource extension method:

    context.Countries.SeedFromResource("MyProject.SeedData.countries.csv", c => c.Code)

You also have the option to seed from a csv file on disk using SeedFromFile, or from any stream using SeedFromStream.

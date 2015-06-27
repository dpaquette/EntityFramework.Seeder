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

##Connecting Related Entities
Loading data from a CSV file can be a little more complicated when there are relationships between the entities were are loading. Take Countries and Provinces/States as an example. A Province/State always belongs to a single Country. In this case, create a CSV file that includes the Country Code of the Country that the Province State belongs to:

    CountryCode,Code,Name
    CA,SK,Saskatchewan
    CA,AB,Alberta
    US,AZ,Arizona
    US,AR,Arkansas
    US,CA,California

Now, specify a custom mapping action when seeding from the provinces csv:

    context.Countries.SeedFromResource("MyProject.SeedData.countries.csv", c => c.Code);
    context.SaveChanges();
    context.ProvinceStates.SeedFromResource("MyProject.SeedData.provincestates.csv", p => p.Code,
            new CsvColumnMapping<ProvinceState>("CountryCode", (state, countryCode) =>
                {
                    state.Country = context.Countries.Single(c => c.Code == countryCode);
                })
             );

##CsvHelper Configuration
Under the covers, EntityFramework.Helper uses CsvHelper to read the CSV files. CsvHelper Configuration can be changed by setting properties on the `Seeder.Configuration` property. For example, to change the delimiter to a | character:


    Seeder.Configuration.Delimiter = "|";

All configuration options available in [CsvHelper Configuration](http://joshclose.github.io/CsvHelper/#configuration) can be set here. Configuration changes are global and will apply to any Seed methods that are called after a configuration option is changed. If you need to reset to the default configuration, call the `Seeder.ResetConfiguration()` method.


##Available on Nuget
PM> Install-Package EntityFramework.Seeder.EF6

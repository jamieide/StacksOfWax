using System;
using System.Data.Entity;
using System.Data.SqlClient;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.Shared.DataAccess
{
    public class StacksOfWaxDbInitializer : DropCreateDatabaseAlways<StacksOfWaxDbContext>
    {
        protected override void Seed(StacksOfWaxDbContext context)
        {
            SeedData(context);
        }

        private static void SeedData(StacksOfWaxDbContext context)
        {
            var abb = context.Artists.Add(new Artist("The Allman Brothers Band"));
            context.Albums.Add(new Album(abb, "The Allman Brothers Band"));
            context.Albums.Add(new Album(abb, "Idlewild South"));
            context.Albums.Add(new Album(abb, "At Fillmore East"));
            context.Albums.Add(new Album(abb, "Eat A Peach"));

            var datd = context.Artists.Add(new Artist("Derek & The Dominoes"));
            context.Albums.Add(new Album(datd, "Layla and Other Assorted Love Songs"));

            var hm = new Artist("Herbie Mann");
            context.Albums.Add(new Album(hm, "Push Push"));

            var cb = new Artist("Cowboy");
            context.Albums.Add(new Album(cb, "5'll Getcha Ten"));

            var af = new Artist("Aretha Franklin");
            context.Albums.Add(new Album(af, "Soul '69"));
            context.Albums.Add(new Album(af, "Spirit in the Dark"));

            var db = new Artist("Delaney & Bonnie");
            context.Albums.Add(new Album(db, "To Bonnie From Delaney"));
            context.Albums.Add(new Album(db, "Motel Shot"));

            var jj = new Artist("Johnny Jenkins");
            context.Albums.Add(new Album(jj, "Ton-Ton Macoute!"));

            context.SaveChanges();
        }

    }
}
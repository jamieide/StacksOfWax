using System.Data.Entity;
using StacksOfWax.Shared.Models;

namespace StacksOfWax.Shared.DataAccess
{
    public class StacksOfWaxDbInitializer : DropCreateDatabaseAlways<StacksOfWaxDbContext>
    {
        protected override void Seed(StacksOfWaxDbContext context)
        {
            var abb = context.Artists.Add(new Artist("The Allman Brothers Band"));
            context.Albums.Add(new Album(abb, "The Allman Brothers Band"));
            context.Albums.Add(new Album(abb, "Idlewild South"));

            context.SaveChanges();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Data
{
    public class DbInitializer
    {
        public static void Initialize(DBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Tests.Any())
            {
                return;   // DB has been seeded
            }


            context.SaveChanges();

            
        }
    }
}

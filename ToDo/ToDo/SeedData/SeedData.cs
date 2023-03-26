using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Data;
using System;
using System.Linq;
namespace ToDo.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoContext(
                       serviceProvider.GetRequiredService<
                           DbContextOptions<ToDoContext>>()))
            {
                // Look for any movies.
                if (context.ListEntry.Any())
                {
                    return;   // DB has been seeded
                }
                context.ListEntry.AddRange(

                    new ListEntry
                    {
                        Description = "Start ToDo List",
                        isDone = false
                    }
                );
                context.SaveChanges();
            }
        }
    }

}

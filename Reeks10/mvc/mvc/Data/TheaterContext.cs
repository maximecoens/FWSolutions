using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Data
{
    public class TheaterContext : DbContext
    {
        public TheaterContext (DbContextOptions<TheaterContext> options)
            : base(options)
        {
        }

        public DbSet<mvc.Models.NewsMessage> NewsMessage { get; set; } = default!;
    }
}

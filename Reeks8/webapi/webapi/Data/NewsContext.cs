using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext (DbContextOptions<NewsContext> options)
            : base(options)
        {
        }

        public DbSet<webapi.Model.Nieuwsbericht> Nieuwsbericht { get; set; } = default!;
    }
}

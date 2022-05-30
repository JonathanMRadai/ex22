using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ex22;

namespace ex22.Data
{
    public class ex22Context : DbContext
    {
        public ex22Context (DbContextOptions<ex22Context> options)
            : base(options)
        {
        }

        public DbSet<ex22.Contact>? Contact { get; set; }
        public DbSet<ex22.Chat>? Chat { get; set; }
        public DbSet<ex22.Message>? Message { get; set; }
        public DbSet<ex22.Invitation>? Invitation { get; set; }
        public DbSet<ex22.Transfer>? Transfer { get; set; }



    }
}

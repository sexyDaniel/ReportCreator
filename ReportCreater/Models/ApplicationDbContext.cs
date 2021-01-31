using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ReportCreater.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DbConnection") { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }
    }
}

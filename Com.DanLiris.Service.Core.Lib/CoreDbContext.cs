using System;
using System.Collections.Generic;
using System.Text;
using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Com.DanLiris.Service.Core.Lib.Models;

namespace Com.DanLiris.Service.Core.Lib
{
    public class CoreDbContext : BaseDbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Buyer> Buyers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

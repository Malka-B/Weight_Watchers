using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Measure.Data.Entities;

namespace Measure.Data
{
    public class MeasureContext : DbContext
    {
        public MeasureContext(DbContextOptions<MeasureContext> options) : base(options)
        {

        }
        public MeasureContext()
        { }

        public DbSet<MeasureEntity> Measures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MeasureEntity>()
                .Property(p => p.Weight)
                .HasColumnType("decimal(18,4)");
        }
    }
}

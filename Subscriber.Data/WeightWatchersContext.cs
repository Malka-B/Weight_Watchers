using Microsoft.EntityFrameworkCore;
using Subscriber.Data.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber.Data
{
    public class WeightWatchersContext : DbContext
    {
        public WeightWatchersContext(DbContextOptions<WeightWatchersContext> options) : base(options)
        {

        }
        public WeightWatchersContext()
        { }

        public DbSet<SubscriberEntity> Subscriber { get; set; }
        public DbSet<CardEntity> Card { get; set; }
        public DbSet<TrackingEntity> Trackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SubscriberEntity>()
            //    .HasOne(b => b.CardEntity)
            //    .WithOne(i => i.SubscriberEntity)
            //    .HasForeignKey<CardEntity>(b => b.SubscriberId);
        }
    }
}

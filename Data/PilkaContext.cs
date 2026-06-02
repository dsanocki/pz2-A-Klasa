using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Aklasa.Models;

namespace Aklasa.Data
{
    public class PilkaContext : DbContext
    {
        public PilkaContext (DbContextOptions<PilkaContext> options)
            : base(options)
        {
        }

        public DbSet<Aklasa.Models.Druzyna> Druzyna { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Zapobieganie konfliktom podwójnych kluczy obcych w SQLite
            modelBuilder.Entity<Mecz>().HasOne(m => m.Gospodarz).WithMany().HasForeignKey(m => m.GospodarzId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Mecz>().HasOne(m => m.Gosc).WithMany().HasForeignKey(m => m.GoscId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transfer>().HasOne(t => t.DruzynaOd).WithMany().HasForeignKey(t => t.DruzynaOdId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transfer>().HasOne(t => t.DruzynaDo).WithMany().HasForeignKey(t => t.DruzynaDoId).OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Aklasa.Models.Zawodnik> Zawodnik { get; set; } = default!;
        public DbSet<Aklasa.Models.Mecz> Mecz { get; set; } = default!;
        public DbSet<Aklasa.Models.Transfer> Transfer { get; set; } = default!;
        public DbSet<Uzytkownik> Uzytkownik { get; set; } = default!;
    }
}

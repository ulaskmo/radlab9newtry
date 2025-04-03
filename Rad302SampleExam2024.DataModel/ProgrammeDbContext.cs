using Microsoft.EntityFrameworkCore;

namespace Rad302SampleExam2024.DataModel
{
    public class ProgrammeDbContext : DbContext
    {
        public ProgrammeDbContext(DbContextOptions<ProgrammeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Programme> Programmes { get; set; }
        public DbSet<ProgrammeDelivery> ProgrammeDeliveries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for ProgrammeDelivery
            modelBuilder.Entity<ProgrammeDelivery>()
                .HasKey(pd => new { pd.ProgCode, pd.ModuleCode });

            modelBuilder.Entity<ProgrammeDelivery>()
                .HasOne(pd => pd.associatedProgramme)
                .WithMany(p => p.programmeDeliveries)
                .HasForeignKey(pd => pd.ProgCode);

            modelBuilder.Entity<ProgrammeDelivery>()
                .HasOne(pd => pd.associatedModule)
                .WithMany(m => m.moduleDeliveries)
                .HasForeignKey(pd => pd.ModuleCode);
        }
    }
}
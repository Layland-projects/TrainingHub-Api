using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using TrainingHub.Infrastructure.Abstractions;
using TrainingHub.Models.Models.Shared;

namespace TrainingHub.Infrastructure
{
    public class TrainingHubContext : IdentityDbContext
    {
        private readonly ITimestampService timestampService;
        public TrainingHubContext(
            ITimestampService timestampService)
        {
            this.timestampService = timestampService;
        }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries<AuditableEntityBase>();
            var updateT = Task.Run(() => UpdateEntities(entities));
            updateT.Wait();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var entities = ChangeTracker.Entries<AuditableEntityBase>();
            var updateT = Task.Run(() => UpdateEntities(entities));
            updateT.Wait();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<AuditableEntityBase>();
            await UpdateEntities(entities);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries<AuditableEntityBase>();
            await UpdateEntities(entities);
            return await base.SaveChangesAsync(cancellationToken);
        }

        private Task UpdateEntities(IEnumerable<EntityEntry<AuditableEntityBase>> entities)
        {
            if (entities != null && entities.Any())
            {
                foreach (var ent in entities)
                {
                    if (ent.State == EntityState.Added)
                    {
                        ent.Entity.CreatedAt = timestampService.GetUTCDateTimeOffset();
                        ent.Entity.UpdatedAt = ent.Entity.CreatedAt;
                    }
                    if (ent.State == EntityState.Modified)
                    {
                        ent.Entity.UpdatedAt = ent.Entity.UpdatedAt;
                    }
                }
            }
            return Task.CompletedTask;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EntityBase>()
                .HasKey(x => x.Id);
            builder.Entity<AuditableEntityBase>()
                .Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.Entity<AuditableEntityBase>()
                .Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
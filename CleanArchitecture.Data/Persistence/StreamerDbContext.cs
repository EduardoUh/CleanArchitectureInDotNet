using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class StreamerDbContext(DbContextOptions<StreamerDbContext> options) : DbContext(options)
    {
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-57LUVQT;Initial Catalog=Streamer;User Id=admin;password=@PlundereR12;TrustServerCertificate=True;");
                // config to show the queries performed by entity framework in the console.
                // .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
                // .EnableSensitiveDataLogging();
        }*/

        // The Fluent API is used when the entity framework conventions for naming foreign keys were not followed, so you will explicitly indicate
        // the type of relation the entities have
        // here is an example, though it is no necessary here because the conventions for naming foreign keys were followed
        // for example lets say that in the Video class the foreign key attribute to Streamer is named as StreamerFKId and not StreamerId
        // in that situation entity framework will not recognize that attribute as a foreign key so you will need to to the following

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationDate = DateTime.Now;
                        entry.Entity.CreatedBy = "system";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastUpdatedDate = DateTime.Now;
                        entry.Entity.LastUpdatedby = "system";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // here we are stating that the entity Streamer
            modelBuilder.Entity<Streamer>()
                // has many instances of Videos
                .HasMany(m => m.Videos)
                // those videos has one streamer
                .WithOne(m => m.Streamer)
                // and the foreign key in the videos is StreamerId
                .HasForeignKey(m => m.StreamerId /*m.StreamerFKId (for example porpuses)*/)
                // it is required
                .IsRequired()
                // setting a cascade behavior
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Director>()
                .HasMany(d => d.Videos)
                .WithOne(d => d.Director)
                .HasForeignKey(d => d.DirectorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Declaring the relation between Video and Actor
            // entity Video
            modelBuilder.Entity<Video>()
                // can have many actors
                .HasMany(a => a.Actors)
                // and those actors can have many videos
                .WithMany(v => v.Videos)
                // and they will be usign VideoActor as the table where there are goin the be coexisting together
                .UsingEntity<VideoActor>(
                // setting up the relations logic in the VideoActor table
                    va => va
                    // a actor can be several times in the VideActor table
                    .HasOne(a => a.Actor)
                    .WithMany(a => a.VideoActors)
                    .HasForeignKey(a => a.ActorId),
                    va => va
                    // a video can be several times in the VideoActor table
                    .HasOne(v => v.Video)
                    .WithMany(v => v.VideoActors)
                    .HasForeignKey(v => v.VideoId),
                    // setting up the compound primary key, in this case it is compound by the Actor and Video primary key
                    va => va.HasKey(e => new { e.ActorId, e.VideoId })
                );

            // we defined a compound primary key for VideoActor but it is heritaging from BaseDomainModel the attribute Id
            // and if we let it just like that it will cause conflicts thats why we tell it to ignore the heritaged Id attribute
            modelBuilder.Entity<VideoActor>().Ignore(va => va.Id);
        }

        // inside this class is where the domain classes are converted to entities
        // don't forget to add the reference between projects, so you can use the domain classes
        public DbSet<Streamer> Streamers { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
    }
}

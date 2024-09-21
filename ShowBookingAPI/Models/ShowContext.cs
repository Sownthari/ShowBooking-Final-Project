using Microsoft.EntityFrameworkCore;

namespace ShowBooking.Models
{


    public class ShowContext : DbContext
    {
        public ShowContext(DbContextOptions<ShowContext> options) : base(options)
        {
        }

        // DbSets for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<TheatreScreen> TheatreScreens { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieInTheatre> MoviesInTheatre { get; set; }
        public DbSet<TheatreShow> TheatreShows { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        public DbSet<TheatreBooking> TheatreBookings { get; set; }
        public DbSet<TheatreTicket> TheatreTickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defining composite key for MovieInTheatre table
            modelBuilder.Entity<MovieInTheatre>()
                .HasKey(mt => new { mt.MovieID, mt.TheatreID });

            modelBuilder.Entity<MovieInTheatre>()
                .HasOne(mt => mt.Movie)
                .WithMany(m => m.MoviesInTheatre)
                .HasForeignKey(mt => mt.MovieID);

            modelBuilder.Entity<MovieInTheatre>()
                .HasOne(mt => mt.Theatre)
                .WithMany(t => t.MoviesInTheatre)
                .HasForeignKey(mt => mt.TheatreID);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }

}

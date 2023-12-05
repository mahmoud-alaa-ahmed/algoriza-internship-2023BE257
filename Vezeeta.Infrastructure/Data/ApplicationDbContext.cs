using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vezeeta.Core.Models;
using Vezeeta.Infrastructure.Data.DataSeed;

namespace Vezeeta.Infrastructure.Data
{
	public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
	{
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TimeModel> Times { get; set; }
        public DbSet<DayModel> Days { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {  
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            //Seed Roles And Intial Admin
            modelBuilder.SeedUserAndRoles();
		}
	}
}

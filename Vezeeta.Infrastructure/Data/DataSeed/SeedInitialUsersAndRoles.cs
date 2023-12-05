using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vezeeta.Core.Models;

namespace Vezeeta.Infrastructure.Data.DataSeed
{
	public static class SeedInitialUsersAndRoles
	{
		public static void SeedUserAndRoles(this ModelBuilder modelBuilder)
		{

			// Add Roles
			List<IdentityRole> roles = new List<IdentityRole>()
			{
				 new IdentityRole {Id = "1", Name = UserRole.Admin, NormalizedName = "ADMIN" },
				 new IdentityRole {Id = "2", Name = UserRole.Patient, NormalizedName = "PATIENT" },
				 new IdentityRole {Id = "3", Name = UserRole.Doctor, NormalizedName = "DOCTOR" },


			};

			modelBuilder.Entity<IdentityRole>().HasData(roles);

			// Seed Users
			var passwordHasher = new PasswordHasher<ApplicationUser>();

			List<ApplicationUser> users = new List<ApplicationUser>()
			{
				 new ApplicationUser {
					FirstName = "Mahmoud",
					LastName = "Alaa",
					UserName = "mahmoud_alaa",
					Gender = Core.Enums.Gender.Male,
					DateOfBirth = DateTime.Now,
					UserType = UserRole.Admin,
					Phone = "01140938815",
					NormalizedUserName = "MAHMOUD_ALAA",
					Email = "mahmoudalaa72@gmail.com",
					NormalizedEmail = "MAHMOUDALAA72@GMAIL.COM",
				},
			};
			modelBuilder.Entity<ApplicationUser>().HasData(users);

			// Seed UserRoles
			List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

			// Add Password For All Users
			users[0].PasswordHash = passwordHasher.HashPassword(users[0], "Mahmoud_123");
			userRoles.Add(new IdentityUserRole<string>
			{
				UserId = users[0].Id,
				RoleId =
			roles.First(q => q.Name == UserRole.Admin).Id
			});
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
		}
	}
}

using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            builder.HasData(
                new ApplicationUser
                {
                    Id = "2c1c473e-21c8-479d-9c73-aa310c14a146",
                    Email = "eduardoivanuhgamino2@gmail.com",
                    NormalizedEmail = "eduardoivanuhgamino2@gmail.com",
                    Name = "Eduardo",
                    LastName = "Uh",
                    UserName = "EduardoUh",
                    NormalizedUserName = "eduardouh",
                    PasswordHash = hasher.HashPassword(null, "12345678"),
                    EmailConfirmed = true,
                },
                new ApplicationUser
                {
                    Id = "f5f5a933-317c-4050-b35c-9bd942359ace",
                    Email = "juanperez@gmail.com",
                    NormalizedEmail = "juanperez@gmail.com",
                    Name = "Juan",
                    LastName = "Perez",
                    UserName = "JuanPerez",
                    NormalizedUserName = "juanperez",
                    PasswordHash = hasher.HashPassword(null, "12345678"),
                    EmailConfirmed = true,
                }
                );
        }
    }
}

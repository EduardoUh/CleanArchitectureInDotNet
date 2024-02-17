using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = "15fcb1a0-f019-484c-9e10-62c56517321b",
                    UserId = "2c1c473e-21c8-479d-9c73-aa310c14a146"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "958e1a5f-f529-4535-b893-32933f0a6bbd",
                    UserId = "f5f5a933-317c-4050-b35c-9bd942359ace"
                }
                );
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                // Id = "8e445865-a24d-4543-a6c6-9443d041cdb9",
                Name = "Guest",
                NormalizedName = "GUEST"
            },
            new IdentityRole
            {
                // Id = "8e445865-a24d-4543-a6c6-9443d241cdb9",
                Name = "Authenticated",
                NormalizedName = "AUTHENTICATED"
            },
            new IdentityRole
            {
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
                
        );
    }
}
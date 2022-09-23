using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        var hasher = new PasswordHasher<IdentityUser>();
        
        builder.HasData(
            new IdentityUser
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                UserName = "Vova",
                NormalizedUserName = "VOVA",
                Email = "vova@mail.com",
                PasswordHash = hasher.HashPassword(null, "1")
            }
           
                
        );
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
       
        
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "8e445865-a24d-4543-a6c6-9443d241cdb9",  // authenticated
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
            }
           
                
        );
    }
}
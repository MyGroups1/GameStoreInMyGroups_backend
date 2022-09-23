using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        // builder.HasData(
            // new Game
            // {
            //     Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
            //     Title = "Lords of Hellas",
            //     Description = "Lords of Hellas desc",
            //     Price = 157.00,
            //     UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")   
            // },
            // new Game
            // {
            //     Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991871"),
            //     Title = "Transforments: The Game",
            //     Description = "Transforments: The Game desc",
            //     Price = 99.00,
            //     UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")
            // },
            // new Game
            // {
            //     Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991872"),
            //     Title = "The Witcher 3: Wild Hunt",
            //     Description = "The Witcher 3: Wild Hunt desc",
            //     Price = 99.00,
            //     UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")
            // },
            // new Game
            // {
            //     Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991873"),
            //     Title = "Steel Rats. Wreck and Ride",
            //     Description = "Steel Rats. Wreck and Ride desc",
            //     Price = 110.00,
            //     UserId = new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9")
            // },
            // new Game
            // {
            //     Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991874"),
            //     Title = "BattleField 4",
            //     Description = "BattleField 4 desc",
            //     Price = 34.00,
            //     UserId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991850")
            // }
        // );
    }
}
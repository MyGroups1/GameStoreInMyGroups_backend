using Contracts;
using Entities.Models;

namespace Repository.Repositories;

public class GameRepository:RepositoryBase<Game>,IGameRepository
{
    public GameRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
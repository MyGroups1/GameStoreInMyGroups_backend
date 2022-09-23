using System;
using System.Threading.Tasks;
using Contracts;
using Repository.Repositories;

namespace Repository;

    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _repositoryContext;
        private readonly Lazy<IGameRepository> _gameRepository;
        

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _gameRepository = new Lazy<IGameRepository>(() => new GameRepository(repositoryContext));
           
        }

        public IGameRepository Game => _gameRepository.Value;
       
        
        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }

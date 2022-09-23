using System.Threading.Tasks;
using Contracts;

namespace Contracts;

    public interface IRepositoryManager
    {
        IGameRepository Game { get; }
       
        Task SaveAsync();
    }

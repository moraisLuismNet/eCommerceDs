using eCommerceDs.DTOs;
using eCommerceDs.Models;

namespace eCommerceDs.Repository
{
    public interface IMusicGenreRepository<TEntity> : IeCommerceDsRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> SearchByName(string texto);
        Task<IEnumerable<TEntity>> GetSortedByName(bool ascending);

        Task<IEnumerable<TEntity>> GetMusicGenresWithTotalGroups();
        Task<bool> MusicGenreHasGroups(int id);

        Task<IEnumerable<MusicGenreDTO>> Get();


    }
}

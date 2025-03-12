using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface IMusicGenreService : IeCommerceDsService<MusicGenreDTO, MusicGenreInsertDTO, MusicGenreUpdateDTO>
    {

        Task<IEnumerable<MusicGenreDTO>> SearchByName(string texto);
        Task<IEnumerable<MusicGenreDTO>> GetSortedByName(bool ascending);
        Task<IEnumerable<MusicGenreTotalGroupsDTO>> GetMusicGenresWithTotalGroups();
        Task<bool> MusicGenreHasGroups(int id);



    }
}

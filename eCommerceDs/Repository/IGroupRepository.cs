using eCommerceDs.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceDs.Repository
{
    public interface IGroupRepository<TEntity> : IeCommerceDsRepository<TEntity>
    {
        Task<IEnumerable<GroupRecordsDTO>> GetGroupsRecords();
        Task<IEnumerable<TEntity>> SearchByName(string texto);
        Task<IEnumerable<TEntity>> GetSortedByName(bool ascending);
        Task<bool> MusicGenreExists(int id);
        Task<bool> GroupHasRecords(int id);

        Task<IEnumerable<GroupDTO>> Get();
        Task<GroupRecordsDTO> GetRecordsByGroup(int id);

    }
}

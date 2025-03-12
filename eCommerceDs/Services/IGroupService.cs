using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface IGroupService : IeCommerceDsService<GroupDTO, GroupInsertDTO, GroupUpdateDTO>
    {
        Task<IEnumerable<GroupRecordsDTO>> GetGroupsRecords();

        Task<IEnumerable<GroupDTO>> SearchByName(string texto);
        Task<IEnumerable<GroupDTO>> GetSortedByName(bool ascending);
        Task<bool> GroupHasRecords(int id);
        Task<GroupRecordsDTO> GetRecordsByGroup(int idGroup);
    }
}

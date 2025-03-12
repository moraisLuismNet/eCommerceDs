using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface IRecordService : IeCommerceDsService<RecordDTO, RecordInsertDTO, RecordUpdateDTO>
    {
        Task<IEnumerable<RecordDTO>> GetSortedByTitle(bool ascending);
        Task<IEnumerable<RecordDTO>> SearchByTitle(string texto);
        Task<IEnumerable<RecordDTO>> GetByPriceRange(decimal min, decimal max);
        Task<IEnumerable<RecordDTO>> GetGroupedByDiscontinued();
    }
}

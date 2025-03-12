using eCommerceDs.DTOs;

namespace eCommerceDs.Repository
{
    public interface IRecordRepository<TEntity> : IeCommerceDsRepository<TEntity>
    {
        Task<bool> GroupExists(int id);
        Task<IEnumerable<TEntity>> GetSortedByTitle(bool ascending);
        Task<IEnumerable<TEntity>> SearchByTitle(string texto);

        Task<IEnumerable<TEntity>> GetByPriceRange(decimal min, decimal max);
        Task<IEnumerable<object>> GetGroupedByDiscontinued();

        Task<IEnumerable<RecordDTO>> Get();

    }
}

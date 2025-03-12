using eCommerceDs.DTOs;
using eCommerceDs.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Repository
{
    public class RecordRepository : IRecordRepository<Record>
    {
        private readonly eCommerceDsContext _context;

        public RecordRepository(eCommerceDsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecordDTO>> Get()
        {

            var records = await (from x in _context.Records
                                select new RecordDTO
                                {
                                    IdRecord = x.IdRecord,
                                    TitleRecord = x.TitleRecord,
                                    ImageRecord = x.ImageRecord,
                                    GroupId = x.GroupId,
                                    NameGroup = x.Group.NameGroup,
                                    YearOfPublication = x.YearOfPublication,
                                    Price = x.Price,
                                    Stock = x.Stock,
                                    Discontinued = x.Discontinued
                                }).ToListAsync();
            return records;
        }

        public async Task<Record> GetById(int id)
        {
            return await _context.Records.FindAsync(id);
        }

        public async Task<IEnumerable<Record>> GetSortedByTitle(bool ascending) =>
            ascending
                ? await _context.Records.OrderBy(x => x.TitleRecord).ToListAsync()
                : await _context.Records.OrderByDescending(x => x.TitleRecord).ToListAsync();


        public async Task<IEnumerable<Record>> SearchByTitle(string texto) =>
            await _context.Records.Where(x => x.TitleRecord.Contains(texto)).ToListAsync();


        public async Task<IEnumerable<Record>> GetByPriceRange(decimal min, decimal max) =>
            await _context.Records.Where(x => x.Price >= min && x.Price <= max).ToListAsync();


        public async Task<IEnumerable<object>> GetGroupedByDiscontinued() =>

            await _context.Records.GroupBy(l => l.Discontinued)
                .Select(group => new
                {
                    Discontinued = group.Key,
                    Count = group.Count()
                }).ToListAsync();

        public async Task Add(Record entity)
        {
            await _context.Records.AddAsync(entity);
        }

        public async Task<bool> GroupExists(int groupId)
        {
            return await _context.Groups.AnyAsync(g => g.IdGroup == groupId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Record entity)
        {
            _context.Records.Update(entity);
        }

        public void Delete(Record entity)
        {
            _context.Records.Remove(entity);
        }

        public IEnumerable<Record> Search(Func<Record, bool> filter)
        {
            return _context.Records.Where(filter).ToList();
        }

        Task<IEnumerable<Record>> IeCommerceDsRepository<Record>.Get()
        {
            throw new NotImplementedException();
        }

    }
}

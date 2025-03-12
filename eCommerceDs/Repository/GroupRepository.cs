using eCommerceDs.DTOs;
using eCommerceDs.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Repository
{
    public class GroupRepository : IGroupRepository<Group>
    {
        private readonly eCommerceDsContext _context;

        public GroupRepository(eCommerceDsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupDTO>> Get()
        {

            var groups = await (from x in _context.Groups
                                     select new GroupDTO
                                     {
                                         IdGroup = x.IdGroup,
                                         NameGroup = x.NameGroup,
                                         ImageGroup = x.ImageGroup,
                                         MusicGenreId = x.MusicGenreId,
                                         NameMusicGenre = x.MusicGenre.NameMusicGenre,
                                         TotalRecords = x.Records.Count()
                                     }).ToListAsync();
            return groups;
        }

        public async Task<Group> GetById(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<IEnumerable<GroupRecordsDTO>> GetGroupsRecords()
        {
            return await _context.Groups
                .Include(a => a.Records)
                .Select(a => new GroupRecordsDTO
                {
                    IdGroup = a.IdGroup,
                    NameGroup = a.NameGroup,
                    TotalRecords = a.Records.Count,
                    Records = a.Records.Select(l => new RecordItemDTO
                    {
                        IdRecord = l.IdRecord,
                        TitleRecord = l.TitleRecord
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<GroupRecordsDTO> GetRecordsByGroup(int id)
        {
            var group = await _context.Groups
                .Where(g => g.IdGroup == id)
                .Include(g => g.Records)
                .SingleOrDefaultAsync();

            if (group == null)
            {
                Console.WriteLine($"Group with ID {id} not found in the database.");
                return null;
            }

            return new GroupRecordsDTO
            {
                IdGroup = group.IdGroup,
                NameGroup = group.NameGroup,
                TotalRecords = group.Records.Count,
                Records = group.Records.Select(r => new RecordItemDTO
                {
                    IdRecord = r.IdRecord,
                    TitleRecord = r.TitleRecord,
                    YearOfPublication = r.YearOfPublication,
                    ImageRecord = r.ImageRecord,
                    Price = r.Price,
                    Stock = r.Stock
                }).ToList() ?? new List<RecordItemDTO>()
            };
        }

        public async Task<IEnumerable<Group>> SearchByName(string texto) =>
            await _context.Groups.Where(x => x.NameGroup.Contains(texto)).ToListAsync();

        public async Task<IEnumerable<Group>> GetSortedByName(bool ascending) =>
            ascending
                ? await _context.Groups.OrderBy(x => x.NameGroup).ToListAsync()
                : await _context.Groups.OrderByDescending(x => x.NameGroup).ToListAsync();


        public async Task Add(Group entity)
        {
            await _context.Groups.AddAsync(entity);
        }

        public async Task<bool> MusicGenreExists(int musicGenreId)
        {
            return await _context.MusicGenres.AnyAsync(g => g.IdMusicGenre == musicGenreId);
        }

        public void Update(Group entity)
        {
            _context.Groups.Update(entity);
        }

        public void Delete(Group entity)
        {
            _context.Groups.Remove(entity);
        }

        public async Task<bool> GroupHasRecords(int id)
        {
            return await _context.Records.AnyAsync(b => b.GroupId == id);

        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Group> Search(Func<Group, bool> filter)
        {
            return _context.Groups.Where(filter).ToList();
        }

        Task<IEnumerable<Group>> IeCommerceDsRepository<Group>.Get()
        {
            throw new NotImplementedException();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using eCommerceDs.Models;
using eCommerceDs.DTOs;

namespace eCommerceDs.Repository
{
    public class MusicGenreRepository : IMusicGenreRepository<MusicGenre>
    {
        private readonly eCommerceDsContext _context;

        public MusicGenreRepository(eCommerceDsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MusicGenreDTO>> Get()
        {
            var musicGenres = await (from x in _context.MusicGenres
                                 select new MusicGenreDTO
                                 {
                                     IdMusicGenre = x.IdMusicGenre,
                                     NameMusicGenre = x.NameMusicGenre,
                                     TotalGroups = x.Groups.Count()
                                 }).ToListAsync();
            return musicGenres;
        }

        public async Task<MusicGenre> GetById(int id)
        {
            return await _context.MusicGenres.FindAsync(id);
        }

        public async Task<IEnumerable<MusicGenre>> SearchByName(string texto) =>
            await _context.MusicGenres.Where(x => x.NameMusicGenre.Contains(texto)).ToListAsync();

        public async Task<IEnumerable<MusicGenre>> GetSortedByName(bool ascending) =>
            ascending
                ? await _context.MusicGenres.OrderBy(x => x.NameMusicGenre).ToListAsync()
                : await _context.MusicGenres.OrderByDescending(x => x.NameMusicGenre).ToListAsync();

        public async Task<IEnumerable<MusicGenre>> GetMusicGenresWithTotalGroups()
        {
            return await _context.MusicGenres
                .Include(mg => mg.Groups)
                .ToListAsync();
        }

        public async Task Add(MusicGenre entity)
        {
            await _context.MusicGenres.AddAsync(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(MusicGenre entity)
        {
            _context.MusicGenres.Update(entity);
        }

        public void Delete(MusicGenre entity)
        {
            _context.MusicGenres.Remove(entity);
        }

        public async Task<bool> MusicGenreHasGroups(int id)
        {
            return await _context.Groups.AnyAsync(b => b.MusicGenreId == id);
        }

        public IEnumerable<MusicGenre> Search(Func<MusicGenre, bool> filter)
        {
            return _context.MusicGenres.Where(filter).ToList();
        }

        Task<IEnumerable<MusicGenre>> IeCommerceDsRepository<MusicGenre>.Get()
        {
            throw new NotImplementedException();
        }

    }
}

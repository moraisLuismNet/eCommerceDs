using eCommerceDs.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Repository
{
    public class UserRepository: IeCommerceDsRepository<User>
    {
        private readonly eCommerceDsContext _context;

        public UserRepository(eCommerceDsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task Add(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<User> Search(Func<User, bool> filter)
        {
            return _context.Users.Where(filter).ToList();
        }
        
    }
}

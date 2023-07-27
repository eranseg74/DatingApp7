using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            // An implementation without using the automapper. Here we are mapping manually from the AppUser to the MemberDTO
            /* return await _context.Users
                .Where(x => x.UserName == username)
                .Select(user => new MemberDTO{
                    Id = user.Id,
                    UserName = user.UserName,
                    KnownAs = user.KnownAs
                }).SingleOrDefaultAsync(); */

            // Here we are projecting the AppUser parameters to the MemberDTO using the AutoMapper
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _context.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos) // This line is required in order to include the related entity (photos in this case)
                .ToListAsync<AppUser>();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; // The method returns the number of changes made in the DB
        }

        public void Update(AppUser user)
        {
            // If the state is Modified the entity framework will automatically check and update the user data
            // It is debatable if we need it because entity framework will automatically detect changes and update the DB.
            // Here we are just informing the entity framework that the given AppUser was modified
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
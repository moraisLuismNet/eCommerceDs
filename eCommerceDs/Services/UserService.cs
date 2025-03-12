using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class UserService : IeCommerceDsService<UserDTO, UserInsertDTO, UserUpdateDTO>
    {
        private readonly IeCommerceDsRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; } = new List<string>();

        public UserService(IeCommerceDsRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> Get()
        {
            var users = await _userRepository.Get();
            return users.Select(user => _mapper.Map<UserDTO>(user));
        }

        public async Task<UserDTO> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            return user != null ? _mapper.Map<UserDTO>(user) : null;
        }

        public async Task<UserDTO> Add(UserInsertDTO userInsertDTO)
        {
            var user = _mapper.Map<User>(userInsertDTO);
            await _userRepository.Add(user);
            await _userRepository.Save();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Update(int id, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return null;
            _mapper.Map(userUpdateDTO, user);
            _userRepository.Update(user);
            await _userRepository.Save();
            return _mapper.Map<UserDTO>(user);

        }

        public async Task<UserDTO> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user != null)
            {
                var userDTO = _mapper.Map<UserDTO>(user);

                _userRepository.Delete(user);
                await _userRepository.Save();

                return userDTO;
            }
            return null;
           
        }

        public async Task Save()
        {
            await _userRepository.Save();
        }

        public bool Validate(UserInsertDTO userInsertDTO)
        {
            if (string.IsNullOrWhiteSpace(userInsertDTO.Email) || string.IsNullOrWhiteSpace(userInsertDTO.Password))
            {
                Errors.Add("Email and password required");
                return false;
            }
            return true;
        }

        public bool Validate(UserUpdateDTO userUpdateDTO)
        {
            if (string.IsNullOrWhiteSpace(userUpdateDTO.Email) || string.IsNullOrWhiteSpace(userUpdateDTO.Password))
            {
                Errors.Add("Email and password required");
                return false;
            }
            return true;
        }
        
    }
}

using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class GroupService : IGroupService
    {
        private IGroupRepository<Group> _groupRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }
        private readonly IFileManagerService _fileManagerService;

        public GroupService(IGroupRepository<Group> groupRepository,
            IMapper mapper, IFileManagerService fileManagerService)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _fileManagerService = fileManagerService;
        }

        public async Task<IEnumerable<GroupDTO>> Get()
        {
            var groups = await _groupRepository.Get();
            return groups.Select(group => _mapper.Map<GroupDTO>(group));
        }

        public async Task<GroupDTO> GetById(int id)
        {
            var group = await _groupRepository.GetById(id);

            if (group != null)
            {
                var groupDTO = _mapper.Map<GroupDTO>(group);
                return groupDTO;
            }

            return null;
        }

        public async Task<IEnumerable<GroupRecordsDTO>> GetGroupsRecords()
        {
            return await _groupRepository.GetGroupsRecords();
        }

        public async Task<GroupRecordsDTO> GetRecordsByGroup(int idGroup)
        {

            var records = await _groupRepository.GetRecordsByGroup(idGroup);

            if (records != null)
            {
                var groupRecordsDTO = _mapper.Map<GroupRecordsDTO>(records);
                return groupRecordsDTO;
            }

            return null;
        }

        public async Task<IEnumerable<GroupDTO>> SearchByName(string texto)
        {
            var groups = await _groupRepository.SearchByName(texto);
            return groups.Select(group => _mapper.Map<GroupDTO>(group));
        }

        public async Task<IEnumerable<GroupDTO>> GetSortedByName(bool ascending)
        {
            var groups = await _groupRepository.GetSortedByName(ascending);
            return groups.Select(group => _mapper.Map<GroupDTO>(group));
        }
        public async Task<GroupDTO> Add(GroupInsertDTO groupInsertDTO)
        {
            if (!await _groupRepository.MusicGenreExists(groupInsertDTO.MusicGenreId))
            {
                throw new ArgumentException($"The with ID {groupInsertDTO.MusicGenreId} does not exist");
            }

            var group = _mapper.Map<Group>(groupInsertDTO);

            if (groupInsertDTO.Photo is not null)
            {
                group.ImageGroup = await ProcessImage(groupInsertDTO.Photo);
            }

            await _groupRepository.Add(group);
            await _groupRepository.Save();

            return _mapper.Map<GroupDTO>(group);
        }

        public async Task<GroupDTO> Update(int id, GroupUpdateDTO groupUpdateDTO)
        {
            var group = await _groupRepository.GetById(id);
            if (group is null) return null;

            _mapper.Map(groupUpdateDTO, group);

            if (groupUpdateDTO.Photo is not null)
            {
                group.ImageGroup = await ProcessImage(groupUpdateDTO.Photo, group.ImageGroup);
            }

            _groupRepository.Update(group);
            await _groupRepository.Save();

            return _mapper.Map<GroupDTO>(group);
        }

        private async Task<string> ProcessImage(IFormFile photo, string existingImage = null)
        {
            if (!string.IsNullOrWhiteSpace(existingImage))
            {
                await _fileManagerService.DeleteFile(existingImage, "img");
            }

            using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);

            var content = memoryStream.ToArray();
            var extension = Path.GetExtension(photo.FileName);
            var contentType = photo.ContentType;

            return await _fileManagerService.SaveFile(content, extension, "img", contentType);
        }

        public async Task<GroupDTO> Delete(int id)
        {
            var group = await _groupRepository.GetById(id);

            if (group != null)
            {
                var groupDTO = _mapper.Map<GroupDTO>(group);

                if (!string.IsNullOrWhiteSpace(group.ImageGroup))
                {
                    await _fileManagerService.DeleteFile(group.ImageGroup, "img");
                }

                _groupRepository.Delete(group);
                await _groupRepository.Save();

                return groupDTO;
            }
            return null;
        }

        public async Task<bool> GroupHasRecords(int id)
        {
            return await _groupRepository.GroupHasRecords(id);
        }

        public bool Validate(GroupInsertDTO groupInsertDTO)
        {
            if (_groupRepository.Search(b => b.NameGroup == groupInsertDTO.NameGroup).Count() > 0)
            {
                Errors.Add("There is already a Group with that Name");
                return false;
            }
            return true;
        }

        public bool Validate(GroupUpdateDTO groupUpdateDTO)
        {
            if (_groupRepository.Search(b => b.NameGroup == groupUpdateDTO.NameGroup && groupUpdateDTO.IdGroup !=
            b.IdGroup).Count() > 0)
            {
                Errors.Add("There is already a Group with that Name");
                return false;
            }
            return true;
        }

    }
}

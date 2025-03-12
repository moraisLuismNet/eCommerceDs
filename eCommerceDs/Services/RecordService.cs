using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class RecordService : IRecordService
    {
        private IRecordRepository<Record> _recordRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }
        private readonly IFileManagerService _fileManagerService;

        public RecordService(IRecordRepository<Record> recordRepository,
            IMapper mapper,
            IFileManagerService fileManagerService)
        {
            _recordRepository = recordRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _fileManagerService = fileManagerService;
        }
        public async Task<IEnumerable<RecordDTO>> Get()
        {
            var records = await _recordRepository.Get();
            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }

        public async Task<RecordDTO> GetById(int id)
        {
            var record = await _recordRepository.GetById(id);

            if (record != null)
            {
                var recordDTO = _mapper.Map<RecordDTO>(record);
                return recordDTO;
            }

            return null;
        }

        public async Task<IEnumerable<RecordDTO>> GetSortedByTitle(bool ascending)
        {
            var records = await _recordRepository.GetSortedByTitle(ascending);
            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }


        public async Task<IEnumerable<RecordDTO>> SearchByTitle(string texto)
        {
            var records = await _recordRepository.SearchByTitle(texto);
            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }

        public async Task<IEnumerable<RecordDTO>> GetByPriceRange(decimal min, decimal max)
        {
            var records = await _recordRepository.GetByPriceRange(min, max);
            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }
        public async Task<IEnumerable<RecordDTO>> GetGroupedByDiscontinued()
        {
            var records = await _recordRepository.GetGroupedByDiscontinued();
            return records.Select(record => _mapper.Map<RecordDTO>(record));
        }

        public async Task<RecordDTO> Add(RecordInsertDTO recordInsertDTO)
        {
            if (!await _recordRepository.GroupExists(recordInsertDTO.GroupId))
            {
                throw new ArgumentException($"The Group with ID {recordInsertDTO.GroupId} does not exist");
            }

            var record = _mapper.Map<Record>(recordInsertDTO);

            if (recordInsertDTO.Photo is not null)
            {
                record.ImageRecord = await ProcessImage(recordInsertDTO.Photo);
            }

            await _recordRepository.Add(record);
            await _recordRepository.Save();

            return _mapper.Map<RecordDTO>(record);
        }

        public async Task<RecordDTO> Update(int id, RecordUpdateDTO recordUpdateDTO)
        {
            var record = await _recordRepository.GetById(id);
            if (record is null) return null;

            _mapper.Map(recordUpdateDTO, record);

            if (recordUpdateDTO.Photo is not null)
            {
                record.ImageRecord = await ProcessImage(recordUpdateDTO.Photo, record.ImageRecord);
            }

            _recordRepository.Update(record);
            await _recordRepository.Save();

            return _mapper.Map<RecordDTO>(record);
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

        public async Task<RecordDTO> Delete(int id)
        {
            var record = await _recordRepository.GetById(id);

            if (record != null)
            {
                var recordDTO = _mapper.Map<RecordDTO>(record);

                if (!string.IsNullOrWhiteSpace(record.ImageRecord))
                {
                    await _fileManagerService.DeleteFile(record.ImageRecord, "img");
                }

                _recordRepository.Delete(record);
                await _recordRepository.Save();

                return recordDTO;
            }
            return null;
        }

        public bool Validate(RecordInsertDTO recordInsertDTO)
        {
            if (_recordRepository.Search(b => b.TitleRecord == recordInsertDTO.TitleRecord).Count() > 0)
            {
                Errors.Add("There is already a Record with that Title");
                return false;
            }
            return true;
        }

        public bool Validate(RecordUpdateDTO recordUpdateDTO)
        {
            if (_recordRepository.Search(b => b.TitleRecord == recordUpdateDTO.TitleRecord && recordUpdateDTO.IdRecord !=
            b.IdRecord).Count() > 0)
            {
                Errors.Add("There is already a Record with that Title");
                return false;
            }
            return true;
        }

        
    }
}

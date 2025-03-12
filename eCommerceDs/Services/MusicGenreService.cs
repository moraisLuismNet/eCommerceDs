using AutoMapper;
using Microsoft.EntityFrameworkCore;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class MusicGenreService : IMusicGenreService
    {
        private readonly IMusicGenreRepository<MusicGenre> _musicGenreRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public MusicGenreService(IMusicGenreRepository<MusicGenre> musicGenreRepository,
            IMapper mapper)
        {
            _musicGenreRepository = musicGenreRepository;
            _mapper = mapper;
            Errors = new List<string>();

        }

        public async Task<IEnumerable<MusicGenreDTO>> Get()
        {
            var musicGenres = await _musicGenreRepository.Get();
            return musicGenres.Select(musicGenre => _mapper.Map<MusicGenreDTO>(musicGenre));
        }

        public async Task<MusicGenreDTO> GetById(int id)
        {
            var musicGenres = await _musicGenreRepository.GetById(id);

            if (musicGenres != null)
            {
                var musicGenreDTO = _mapper.Map<MusicGenreDTO>(musicGenres);
                return musicGenreDTO;
            }

            return null;
        }

        public async Task<IEnumerable<MusicGenreDTO>> SearchByName(string texto)
        {
            var musicGenres = await _musicGenreRepository.SearchByName(texto);
            return musicGenres.Select(musicGenre => _mapper.Map<MusicGenreDTO>(musicGenre));
        }

        public async Task<IEnumerable<MusicGenreDTO>> GetSortedByName(bool ascending)
        {
            var musicGenres = await _musicGenreRepository.GetSortedByName(ascending);
            return musicGenres.Select(musicGenre => _mapper.Map<MusicGenreDTO>(musicGenre));
        }

        public async Task<IEnumerable<MusicGenreTotalGroupsDTO>> GetMusicGenresWithTotalGroups()
        {
            var musicGenres = await _musicGenreRepository.GetMusicGenresWithTotalGroups();

            return musicGenres.Select(mg => new MusicGenreTotalGroupsDTO
            {
                IdMusicGenre = mg.IdMusicGenre,
                NameMusicGenre = mg.NameMusicGenre,
                TotalGroups = mg.Groups.Count()
            }).ToList();
        }

        public async Task<MusicGenreDTO> Add(MusicGenreInsertDTO musicGenreInsertDTO)
        {
            var musicGenre = _mapper.Map<MusicGenre>(musicGenreInsertDTO);
            await _musicGenreRepository.Add(musicGenre);
            await _musicGenreRepository.Save();
            var musicGenreDTO = _mapper.Map<MusicGenreDTO>(musicGenre);
            return musicGenreDTO;
        }

        public async Task<MusicGenreDTO> Update(int id, MusicGenreUpdateDTO musicGenreUpdateDTO)
        {
            var musicGenre = await _musicGenreRepository.GetById(id);

            if (musicGenre != null)
            {
                musicGenre = _mapper.Map<MusicGenreUpdateDTO, MusicGenre>(musicGenreUpdateDTO, musicGenre);

                _musicGenreRepository.Update(musicGenre);
                await _musicGenreRepository.Save();

                var musicGenreDTO = _mapper.Map<MusicGenreDTO>(musicGenre);

                return musicGenreDTO;
            }
            return null;
        }

        public async Task<MusicGenreDTO> Delete(int id)
        {
            var musicGenre = await _musicGenreRepository.GetById(id);

            if (musicGenre != null)
            {
                var musicGenreDTO = _mapper.Map<MusicGenreDTO>(musicGenre);

                _musicGenreRepository.Delete(musicGenre);
                await _musicGenreRepository.Save();

                return musicGenreDTO;
            }
            return null;
        }

        public async Task<bool> MusicGenreHasGroups(int id)
        {
            return await _musicGenreRepository.MusicGenreHasGroups(id);
        }

        public bool Validate(MusicGenreInsertDTO musicGenreInsertDTO)
        {
            if (_musicGenreRepository.Search(b => b.NameMusicGenre == musicGenreInsertDTO.NameMusicGenre).Count() > 0)
            {
                Errors.Add("There is already a Music Genre with that Name");
                return false;
            }
            return true;
        }
        
        public bool Validate(MusicGenreUpdateDTO musicGenreUpdateDTO)
        {
            if (_musicGenreRepository.Search(b => b.NameMusicGenre == musicGenreUpdateDTO.NameMusicGenre && musicGenreUpdateDTO.IdMusicGenre !=
            b.IdMusicGenre).Count() > 0)
            {
                Errors.Add("There is already a Music Genre with that Name");
                return false;
            }
            return true;
        }

    }
}

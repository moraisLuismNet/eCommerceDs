using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using eCommerceDs.DTOs;
using eCommerceDs.Services;
using Microsoft.AspNetCore.Authorization;

namespace eCommerceDs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MusicGenresController : ControllerBase
    {
        private readonly IValidator<MusicGenreInsertDTO> _musicGenreInsertValidator;
        private readonly IValidator<MusicGenreUpdateDTO> _musicGenreUpdateValidator;
        private readonly IMusicGenreService _musicGenreService;

        public MusicGenresController(IValidator<MusicGenreInsertDTO> musicGenreInsertValidator,
            IValidator<MusicGenreUpdateDTO> musicGenreUpdateValidator,
            IMusicGenreService musicGenreService)
        {
            _musicGenreInsertValidator = musicGenreInsertValidator;
            _musicGenreUpdateValidator = musicGenreUpdateValidator;
            _musicGenreService = musicGenreService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<MusicGenreDTO>> Get() =>
            await _musicGenreService.Get();



        [HttpGet("{IdMusicGenre:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<MusicGenreDTO>> GetById(int IdMusicGenre)
        {
            var musicGenreDTO = await _musicGenreService.GetById(IdMusicGenre);
            return musicGenreDTO == null ? NotFound($"MusicGenre with ID {IdMusicGenre} not found") : Ok(musicGenreDTO);
        }


        [HttpGet("SearchByName/{text}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MusicGenreDTO>>> SearchByName(string text)
        {
            var musicGenres = await _musicGenreService.SearchByName(text);

            if (musicGenres == null || !musicGenres.Any())
            {
                return NotFound($"No musical genres found matching the text '{text}'");
            }

            return Ok(musicGenres);
        }


        [HttpGet("sortedByName/{ascen}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MusicGenreDTO>>> GetSortedByName([FromQuery] bool ascending)
        {
            var musicGenres = await _musicGenreService.GetSortedByName(ascending);

            if (musicGenres == null || !musicGenres.Any())
            {
                return NotFound("No musical genres found");
            }

            return Ok(musicGenres);
        }


        [HttpGet("GetMusicGenresWithTotalGroups")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MusicGenreTotalGroupsDTO>>> GetMusicGenresWithTotalGroups()
        {
            var genres = await _musicGenreService.GetMusicGenresWithTotalGroups();
            return Ok(genres);
        }


        [HttpPost]
        public async Task<ActionResult<MusicGenreDTO>> Add(MusicGenreInsertDTO musicGenreInsertDTO)
        {
            var validationResult = await _musicGenreInsertValidator.ValidateAsync(musicGenreInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_musicGenreService.Validate(musicGenreInsertDTO))
            {
                return BadRequest(_musicGenreService.Errors);
            }

            var musicGenreDTO = await _musicGenreService.Add(musicGenreInsertDTO);

            return CreatedAtAction(nameof(GetById), new { musicGenreDTO.IdMusicGenre }, musicGenreDTO);
        }


        [HttpPut("{IdMusicGenre:int}")]
        public async Task<ActionResult<MusicGenreDTO>> Update(int IdMusicGenre, MusicGenreUpdateDTO musicGenreUpdateDTO)
        {
            var validationResult = await _musicGenreUpdateValidator.ValidateAsync(musicGenreUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_musicGenreService.Validate(musicGenreUpdateDTO))
            {
                return BadRequest(_musicGenreService.Errors);
            }

            var musicGenreDTO = await _musicGenreService.Update(IdMusicGenre, musicGenreUpdateDTO);

            return musicGenreDTO == null ? NotFound($"MusicGenre with ID {IdMusicGenre} not found") : Ok(musicGenreDTO);
        }


        [HttpDelete("{IdMusicGenre:int}")]
        public async Task<ActionResult<MusicGenreDTO>> Delete(int IdMusicGenre)
        {
            bool hasGroups = await _musicGenreService.MusicGenreHasGroups(IdMusicGenre);
            if (hasGroups)
            {
                return BadRequest($"The Music Genre with ID {IdMusicGenre} cannot be deleted because it has associated Groups");
            }
            var musicGenreDTO = await _musicGenreService.Delete(IdMusicGenre);
            return musicGenreDTO == null ? NotFound($"MusicalGenre with ID {IdMusicGenre} not found") : Ok(musicGenreDTO);
        }

    }
}

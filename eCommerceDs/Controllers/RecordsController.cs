using eCommerceDs.DTOs;
using eCommerceDs.Services;
using eCommerceDs.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RecordsController : ControllerBase
    {
        private readonly IValidator<RecordInsertDTO> _recordInsertValidator;
        private readonly IValidator<RecordUpdateDTO> _recordUpdateValidator;
        private readonly IRecordService _recordService;
        public RecordsController(IValidator<RecordInsertDTO> recordInsertValidator, 
            IValidator<RecordUpdateDTO> recordUpdateValidator, IRecordService recordService)
        {
            _recordInsertValidator = recordInsertValidator;
            _recordUpdateValidator = recordUpdateValidator;
            _recordService = recordService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<RecordDTO>> Get() =>
            await _recordService.Get();



        [HttpGet("{IdRecord:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<RecordDTO>> GetById(int IdRecord)
        {
            var recordDTO = await _recordService.GetById(IdRecord);
            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }


        [HttpGet("sortedByTitle/{ascen}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetSortedByTitle(bool ascen)
        {
            var records = await _recordService.GetSortedByTitle(ascen);
            return Ok(records);
        }


        [HttpGet("SearchByTitle/{text}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> SearchByTitle(string text)
        {
            var records = await _recordService.SearchByTitle(text);

            if (records == null || !records.Any())
            {
                return NotFound($"No records found matching the text '{text}'");
            }

            return Ok(records);
        }


        [HttpGet("byPriceRange")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetByPriceRange(decimal min, decimal max)
        {
            var records = await _recordService.GetByPriceRange(min, max);
            return Ok(records);
        }


        [HttpGet("groupedByDiscontinued")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetGroupedByDiscontinued()
        {
            var records = await _recordService.GetGroupedByDiscontinued();
            return Ok(records);
        }


        [HttpPost]
        public async Task<ActionResult<RecordDTO>> Add(RecordInsertDTO recordInsertDTO)
        {
            var validationResult = await _recordInsertValidator.ValidateAsync(recordInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_recordService.Validate(recordInsertDTO))
            {
                return BadRequest(_recordService.Errors);
            }

            var recordDTO = await _recordService.Add(recordInsertDTO);

            return CreatedAtAction(nameof(GetById), new { recordDTO.IdRecord }, recordDTO);
        }


        [HttpPut("{IdRecord:int}")]
        public async Task<ActionResult<RecordDTO>> Update(int IdRecord, RecordUpdateDTO recordUpdateDTO)
        {
            var validationResult = await _recordUpdateValidator.ValidateAsync(recordUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_recordService.Validate(recordUpdateDTO))
            {
                return BadRequest(_recordService.Errors);
            }

            var recordDTO = await _recordService.Update(IdRecord, recordUpdateDTO);

            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }


        [HttpDelete("{IdRecord:int}")]
        public async Task<ActionResult<RecordDTO>> Delete(int IdRecord)
        {
            var recordDTO = await _recordService.Delete(IdRecord);
            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }

    }
}

using eCommerceDs.DTOs;
using eCommerceDs.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            await _recordService.GetService();



        [HttpGet("{IdRecord:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<RecordDTO>> GetById(int IdRecord)
        {
            var recordDTO = await _recordService.GetByIdService(IdRecord);
            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }


        [HttpGet("sortedByTitle/{ascen}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetSortedByTitle(bool ascen)
        {
            var records = await _recordService.GetSortedByTitleRecordService(ascen);
            return Ok(records);
        }


        [HttpGet("SearchByTitle/{text}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> SearchByTitle(string text)
        {
            var records = await _recordService.SearchByTitleRecordService(text);

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
            var records = await _recordService.GetByPriceRangeRecordService(min, max);
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

            var recordDTO = await _recordService.AddService(recordInsertDTO);

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

            var recordDTO = await _recordService.UpdateService(IdRecord, recordUpdateDTO);

            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }


        [HttpPut("{id:int}/updateStock/{amount:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStock(int id, int amount)
        {
            var record = await _recordService.GetByIdService(id);

            if (record == null)
            {
                return NotFound(new { message = $"Record with ID {id} not found" });
            }

            try
            {
                await _recordService.UpdateStockRecordService(id, amount);
                var updatedRecord = await _recordService.GetByIdService(id);
                return Ok(new
                {
                    message = $"The stock of the record with ID {id} has been updated in {amount} units",
                    newStock = updatedRecord.Stock
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("{IdRecord:int}")]
        public async Task<ActionResult<RecordDTO>> Delete(int IdRecord)
        {
            var recordDTO = await _recordService.DeleteService(IdRecord);
            return recordDTO == null ? NotFound($"Record with ID {IdRecord} not found") : Ok(recordDTO);
        }

    }
}

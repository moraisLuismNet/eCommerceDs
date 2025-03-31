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
    public class GroupsController : ControllerBase
    {
        private readonly IValidator<GroupInsertDTO> _groupInsertValidator;
        private readonly IValidator<GroupUpdateDTO> _groupUpdateValidator;
        private readonly IGroupService _groupService;

        public GroupsController(IValidator<GroupInsertDTO> groupInsertValidator,
            IValidator<GroupUpdateDTO> groupUpdateValidator,
            IGroupService groupService)
        {
            _groupInsertValidator = groupInsertValidator;
            _groupUpdateValidator = groupUpdateValidator;
            _groupService = groupService;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<GroupDTO>> Get() =>
            await _groupService.GetService();

        

        [HttpGet("{IdGroup:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<GroupDTO>> GetById(int IdGroup)
        {
            var groupDTO = await _groupService.GetByIdService(IdGroup);
            return groupDTO == null ? NotFound($"Group with ID {IdGroup} not found") : Ok(groupDTO);
        }


        [HttpGet("groupsRecords")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GroupRecordsDTO>>> GetGroupsRecords()
        {
            var groupRecords = await _groupService.GetGroupsRecordsGroupService();
            return Ok(groupRecords);
        }


        [HttpGet("recordsByGroup/{idGroup}")]
        [AllowAnonymous]
        public async Task<ActionResult<GroupRecordsDTO>> GetRecordsByGroup(int idGroup)
        {
            var groupDTO = await _groupService.GetRecordsByGroupGroupService(idGroup);

            if (groupDTO == null)
            {
                return NotFound($"Group with ID {idGroup} not found");
            }

            return Ok(groupDTO);
        }


        [HttpGet("SearchByName/{text}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> SearchByName(string text)
        {
            var groups = await _groupService.SearchByNameGroupService(text);

            if (groups == null || !groups.Any())
            {
                return NotFound($"No groups found matching the text '{text}'");
            }

            return Ok(groups);
        }


        [HttpGet("sortedByName/{ascen}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetSortedByName(bool ascen)
        {
            var groups = await _groupService.GetSortedByNameGroupService(ascen);
            return Ok(groups);
        }


        [HttpPost]
        public async Task<ActionResult<GroupDTO>> Add(GroupInsertDTO groupInsertDTO)
        {
            var validationResult = await _groupInsertValidator.ValidateAsync(groupInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var groupDTO = await _groupService.AddService(groupInsertDTO);

            return CreatedAtAction(nameof(GetById), new { groupDTO.IdGroup }, groupDTO);
        }


        [HttpPut("{IdGroup:int}")]
        public async Task<ActionResult<GroupDTO>> Update(int IdGroup, GroupUpdateDTO groupUpdateDTO)
        {
            var validationResult = await _groupUpdateValidator.ValidateAsync(groupUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var groupDTO = await _groupService.UpdateService(IdGroup, groupUpdateDTO);

            return groupDTO == null ? NotFound($"Group with ID {IdGroup} not found") : Ok(groupDTO);
        }


        [HttpDelete("{IdGroup:int}")]
        public async Task<ActionResult<GroupDTO>> Delete(int IdGroup)
        {
            bool hasGroups = await _groupService.GroupHasRecordsGroupService(IdGroup);
            if (hasGroups)
            {
                return BadRequest($"The Group with ID {IdGroup} cannot be deleted because it has associated Records");
            }
            var groupDTO = await _groupService.DeleteService(IdGroup);
            return groupDTO == null ? NotFound($"Group with ID {IdGroup} not found") : Ok(groupDTO);
        }
    }
}

using System.Net;
using System.Net.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_api.Controllers
{
    [Authorize(Roles = "Player,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        //Comment : int skill id should be changed into a relevant type when implementing
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetSkillDto>>>> AddSkill(AddSkillDto newSkill){
                return Ok(await _skillService.AddSkill(newSkill));
        }

        [HttpDelete("{skillId}")]
        public async Task<ActionResult<ServiceResponse<GetWeaponDto>>> DeleteSkill(int skillId){
                return Ok(await _skillService.DeleteSkill(skillId));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetWeaponDto>>> UpdateSkill(int skillId){
                return Ok(await _skillService.UpdateSkill(skillId));
        }
    }
}
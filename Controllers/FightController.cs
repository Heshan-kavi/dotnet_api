using System.Net;
using System.Net.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_api.Controllers
{
    [Route("Fight/[controller]")]
    public class FightController : Controller
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request){
            var response = await _fightService.WeaponAttack(request);
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request){
            var response = await _fightService.SkillAttack(request);
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SkillCOntroller : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public SkillCOntroller(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon){
                return Ok(await _weaponService.AddWeapon(newWeapon));
        }

        [HttpDelete("{skillId}")]
        public async Task<ActionResult<ServiceResponse<GetWeaponDto>>> DeleteWeapon(int skillId){
                return Ok(await _weaponService.DeleteWeapon(skillId));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetWeaponDto>>> UpdateWeapon(UpdateWeaponDto existingWeapon){
                return Ok(await _weaponService.UpdateWeapon(existingWeapon));
        }
    }
}
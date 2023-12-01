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
    [Authorize(Roles = "Player,Admin") ]
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : Controller
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("GetAllSkills")]
        public async Task<ActionResult<ServiceResponse<List<GetSkillDto>>>> GetSkillsByUser (){
            var response = await _skillService.GetSkillsByUser();
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
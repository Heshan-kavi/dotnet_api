using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_api.Controllers
{
    [Authorize(Roles = "Player,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get(){
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingleCharacter(int id){
            return Ok(await _characterService.GetSingleCharacter(id));
        }

        [HttpGet("Skill/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterSkills(int id){
            return Ok(await _characterService.GetCharacterSkills(id));
        }

        [HttpGet("Weapon/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterWeapon(int id){
            return Ok(await _characterService.GetCharacterWeapon(id));
        }

        [HttpPost()]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter){
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut()]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter){
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id){
            var response = await _characterService.DeleteCharacter(id);
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddSkill(AddCharacterSkillDto newCharacterSkill){
            var response = await _characterService.AddSkill(newCharacterSkill);
            if(!response.Success){
                return NotFound(response);
            }
            return Ok(response);
        }
        
    }
}
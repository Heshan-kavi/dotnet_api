using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter);
        Task<ServiceResponse<GetCharacterDto>> UpdateCharacter (UpdateCharacterDto updatedCharacter);
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters ();
        Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter (int id);
        Task<ServiceResponse<GetCharacterDto>> GetCharacterSkills (int id);
        Task<ServiceResponse<GetCharacterDto>> GetCharacterWeapon (int id);
        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter (int id);
        Task<ServiceResponse<GetCharacterDto>> AddSkill (AddCharacterSkillDto newCharacterSkill);
    }
}
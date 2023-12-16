using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.SkillService
{
    public interface ISkillService
    {
        // comment : this should be changed to A dto in the skill file
        Task<ServiceResponse<List<GetSkillDto>>> AddSkill (AddSkillDto newSkill);
        Task<ServiceResponse<GetSkillDto>> UpdateSkill (UpdateSkillDto existingSkill);
        Task<ServiceResponse<GetSkillDto>> DeleteSkill (int skillId);
        Task<ServiceResponse<List<GetSkillDtoWithCharacters>>> GetSkillsAll ();
    }
}
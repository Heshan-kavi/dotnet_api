using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.SkillService
{
    public interface ISkillService
    {
        // comment : this should be changed to A dto in the skill file
        Task<ServiceResponse<GetSkillDto>> AddSkill (int skillId);
        Task<ServiceResponse<GetSkillDto>> UpdateSkill (int skillId);
        Task<ServiceResponse<GetSkillDto>> DeleteSkill (int skillId);
    }
}
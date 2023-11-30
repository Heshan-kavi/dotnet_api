using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.SkillService
{
    public interface ISkillService
    {
        Task<ServiceResponse<List<GetSkillDto>>> GetSkillsByUser();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.Fight
{
    public interface IFightService
    {
        Task<ServiceResponse<GetCharacterDto>> GetFights();
    }
}
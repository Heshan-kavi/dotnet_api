using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        public Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon){
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            return serviceResponse;
        }
    }
}
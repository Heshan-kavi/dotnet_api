using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
        Task<ServiceResponse<GetWeaponDto>> DeleteWeapon(int weaponId);
        Task<ServiceResponse<GetWeaponDto>> UpdateWeapon(UpdateWeaponDto existingWeapon);
    }
}
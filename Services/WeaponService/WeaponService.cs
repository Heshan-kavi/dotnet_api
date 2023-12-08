using System.Runtime.CompilerServices;
using System.Threading;
using System.ComponentModel;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor){
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        } 

        private int GetUserId () => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon){
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
                var character = await _context.Characters.FirstOrDefaultAsync(character => character.Id == newWeapon.CharacterId && character.User.Id == GetUserId());
                if(character is null){
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Couldn't do the add weapon operations at the moment!!!";

                    return serviceResponse;
                }
                _context.Weapons.Add(_mapper.Map<Weapon>(newWeapon)); 
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetWeaponDto>> DeleteWeapon(int weaponId){
            var serviceResponse = new ServiceResponse<GetWeaponDto>();
            try{
                var weapon = await _context.Weapons.FirstOrDefaultAsync(weapon => weapon.Id == weaponId && weapon.Character.User.Id == GetUserId());
                if(weapon is null){
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Couldn't do the delete weapon operations at the moment!!!";

                    return serviceResponse;
                }
                _context.Weapons.Remove(weapon); 
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetWeaponDto>(weapon);
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetWeaponDto>> UpdateWeapon(UpdateWeaponDto existingWeapon){
            var serviceResponse = new ServiceResponse<GetWeaponDto>();
            try{
                var weapon = await _context.Weapons.FirstOrDefaultAsync(weapon => weapon.Id == existingWeapon.Id && weapon.Character.User.Id == GetUserId());
                if(weapon is null){
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Couldn't do the update weapon operations at the moment!!!";

                    return serviceResponse;
                }
                weapon.Name = existingWeapon.Name;
                weapon.Damage = existingWeapon.Damage;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetWeaponDto>(weapon);
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
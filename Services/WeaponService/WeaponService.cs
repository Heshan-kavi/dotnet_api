global using AutoMapper;
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
    }
}
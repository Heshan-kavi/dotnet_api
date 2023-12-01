global using AutoMapper;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private static List<Character> characters = new List<Character>{ 
            new Character(),
            new Character{
                Name = "Sam",
                Id = 1
            }
        };

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId () => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        private string GetUserRole () => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter){
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            var userId = GetUserId();
            character.User = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters
                .Where(character => character.User.Id == userId)
                .Select(character => _mapper.Map<GetCharacterDto>(character))
                .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter (UpdateCharacterDto updatedCharacter){
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
                var character = await _context.Characters
                                    .Include(character => character.Weapon)
                                    .Include(character => character.Skills)
                                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User.Id == GetUserId());

                if(character is null){
                    throw new Exception($"Your requested character not found in here !!!");
                }

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                serviceResponse.Message = "Relevant Character updated successfully !!!";
                return serviceResponse;
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters (){
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var returnedCharacters = GetUserRole() == "Admin" ? 
                                    await _context.Characters
                                        .Include(character => character.Weapon)
                                        .Include(character => character.Skills)
                                        .ToListAsync()
                                    :
                                    await _context.Characters
                                        .Include(character => character.Weapon)
                                        .Include(character => character.Skills)
                                        .Where(character => character.User.Id == GetUserId())
                                        .ToListAsync();
            serviceResponse.Data = returnedCharacters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter (int id){
            var character = await _context.Characters
                                .Include(character => character.Weapon)
                                .Include(character => character.Skills)
                                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            serviceResponse.Success = character is not null ? true : false;
            serviceResponse.Message = character is not null ? "We found the character for you" : "We couldn't find the character that you're looking for";
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterSkills (int id){
            var character = await _context.Characters
                                .Include(character => character.Skills)
                                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            serviceResponse.Success = character is not null ? true : false;
            serviceResponse.Message = character is not null ? "We found the character for you" : "We couldn't find the character that you're looking for";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter (int id){
            var userId = GetUserId();
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try{
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == userId);

                if(character is null){
                throw new Exception($"Cannot identify this character to delelete !!!");
                }

                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = await _context.Characters
                    .Where(character => character.User.Id == userId)
                    .Select(character => _mapper.Map<GetCharacterDto>(character)).ToListAsync();
                serviceResponse.Message = "We deleted the character for you";
                return serviceResponse;
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddSkill (AddCharacterSkillDto newCharacterSkill){
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
                var character = await _context.Characters
                                    .Include(character => character.Weapon)
                                    .Include(character => character.Skills)
                                    .FirstOrDefaultAsync(character => character.Id == newCharacterSkill.CharacterId && character.User.Id == GetUserId());
                if(character is null){
                    throw new Exception($"Cannot find the Character to add the new skill");
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(skill => skill.Id == newCharacterSkill.SkillId);
                if(skill is null){
                    throw new Exception($"Not a valid skill to add");
                }
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;

                return serviceResponse;
            }
        }
    }
}
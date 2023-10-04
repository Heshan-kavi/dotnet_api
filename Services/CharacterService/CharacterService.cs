global using AutoMapper;
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

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter){
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(character => character.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter (UpdateCharacterDto updatedCharacter){
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                if(character is null){
                    throw new Exception($"Your requested character not found in here !!!");
                }

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

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
            serviceResponse.Data = characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetCharacterDto>> GetSingleCharacter (int id){
            var character = characters.FirstOrDefault(c => c.Id == id);
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            serviceResponse.Success = character is not null ? true : false;
            serviceResponse.Message = character is not null ? "We found the character for you" : "We couldn't find the character that you're looking for";
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter (int id){
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try{
                var character = characters.FirstOrDefault(c => c.Id == id);

                if(character is null){
                throw new Exception($"Cannot identify this character to delelete !!!");
                }

                characters.Remove(character);
                serviceResponse.Data = characters.Select(character => _mapper.Map<GetCharacterDto>(character)).ToList();
                serviceResponse.Message = "We deleted the character for you";
                return serviceResponse;
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }
    }
}
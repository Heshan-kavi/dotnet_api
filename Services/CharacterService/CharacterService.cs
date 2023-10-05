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

        public async Task<ServiceResponse<List<Character>>> AddCharacter (Character newCharacter){
            var serviceResponse = new ServiceResponse<List<Character>>();
            characters.Add(newCharacter);
            serviceResponse.Data = characters;
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Character>>> GetAllCharacters (){
            var serviceResponse = new ServiceResponse<List<Character>>();
            serviceResponse.Data = characters;
            return serviceResponse;
        }
        
        public async Task<ServiceResponse<Character>> GetSingleCharacter (int id){
            var character = characters.FirstOrDefault(c => c.Id == id);
            var serviceResponse = new ServiceResponse<Character>();
            serviceResponse.Data = character;
            serviceResponse.Success = character is not null ? true : false;
            serviceResponse.Message = character is not null ? "We found the character for you" : "We couldn't find the character that you're looking for";
            return serviceResponse;
        }
    }
}
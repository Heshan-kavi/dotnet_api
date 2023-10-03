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

        public async Task<List<Character>> AddCharacter (Character newCharacter){
            characters.Add(newCharacter);
            return characters;
        }

        public async Task<List<Character>> GetAllCharacters (){
            return characters;
        }
        
        public async Task<Character> GetSingleCharacter (int id){
            var character = characters.FirstOrDefault(c => c.Id == id);
            if(character is not null){
                return character;
            }

            throw new Exception("Character not found in this occasion");
        }
    }
}
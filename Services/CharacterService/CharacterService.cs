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

        public List<Character> AddCharacter (Character newCharacter){
            characters.Add(newCharacter);
            return characters;
        }

        public List<Character> GetAllCharacters (){
            return characters;
        }
        
        public Character GetSingleCharacter (int id){
            return characters.FirstOrDefault(c => c.Id == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<Character>> AddCharacter (Character newCharacter);
        Task<List<Character>> GetAllCharacters ();
        Task<Character> GetSingleCharacter (int id);
    }
}
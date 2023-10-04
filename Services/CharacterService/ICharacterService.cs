using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.CharacterService
{
    public interface ICharacterService
    {
        List<Character> AddCharacter (Character newCharacter);
        List<Character> GetAllCharacters ();
        Character GetSingleCharacter (int id);
    }
}
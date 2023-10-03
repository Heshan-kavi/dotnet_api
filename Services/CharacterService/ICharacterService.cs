using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<Character>>> AddCharacter (Character newCharacter);
        Task<ServiceResponse<List<Character>>> GetAllCharacters ();
        Task<ServiceResponse<Character>> GetSingleCharacter (int id);
    }
}
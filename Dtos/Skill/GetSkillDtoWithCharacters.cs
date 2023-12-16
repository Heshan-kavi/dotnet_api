using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Dtos.Skill
{
    public class GetSkillDtoWithCharacters
    {
        public string Name { get; set; }
        public int Damage {get; set; }
        public List<GetCharacterDto>? Characters { get; set; }
    }
}
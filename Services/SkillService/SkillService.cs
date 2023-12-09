using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SkillService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<GetSkillDto>> AddSkill (int id){
            Console.WriteLine("comes to the add skill function!");
            return null;
        }

        public async Task<ServiceResponse<GetSkillDto>> UpdateSkill (int id){
            Console.WriteLine("comes to the update skill function!");
            return null;
        }

        public async Task<ServiceResponse<GetSkillDto>> DeleteSkill (int id){
            Console.WriteLine("comes to the delete skill function!");
            return null;
        }
    }
}
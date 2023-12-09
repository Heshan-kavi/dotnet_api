using System.Net.Sockets;
using System.Security.Claims;
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

        private int GetUserId () => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public async Task<ServiceResponse<List<GetSkillDto>>> AddSkill (AddSkillDto newSkill){
            var serviceResponse = new ServiceResponse<List<GetSkillDto>>();

            try{
                _context.Skills.Add(_mapper.Map<Skill>(newSkill));
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Skills
                    .Select(skill => _mapper.Map<GetSkillDto>(skill))
                    .ToListAsync();
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
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
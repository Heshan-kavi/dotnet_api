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

        private string GetUserRole () => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

        public async Task<ServiceResponse<List<GetSkillDto>>> AddSkill (AddSkillDto newSkill){
            var serviceResponse = new ServiceResponse<List<GetSkillDto>>();

            try{
                if(GetUserRole() != "Admin"){
                    throw new Exception("Only Admin allowed to add a new skill");
                }
                Console.WriteLine(GetUserRole());
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

        public async Task<ServiceResponse<GetSkillDto>> UpdateSkill (UpdateSkillDto existingSkill){
            var serviceResponse = new ServiceResponse<GetSkillDto>();

            try{
                if(GetUserRole() != "Admin"){
                    throw new Exception("Only Admin allowed to update a skill");
                }
                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == existingSkill.SkillId);
                if(skill is null){
                    throw new Exception($"Your requested skill not found in here !!!");
                }

                skill.Name = existingSkill.Name;
                skill.Damage = existingSkill.Damage;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetSkillDto>(skill);
                serviceResponse.Message = "Relevant Skill updated successfully !!!";
                return serviceResponse;
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<GetSkillDto>> DeleteSkill (int skillId){
            var serviceResponse = new ServiceResponse<GetSkillDto>();

            try{
                if(GetUserRole() != "Admin"){
                    throw new Exception("Only Admin allowed to delete a skill");
                }
                var skill = await _context.Skills.FirstOrDefaultAsync(skill => skill.Id == skillId);
                if(skill is null){
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Couldn't do the delete skill operations at the moment!!!";

                    return serviceResponse;
                }
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetSkillDto>(skill);
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetSkillDtoWithCharacters>>> GetSkillsAll (){
            var serviceResponse = new ServiceResponse<List<GetSkillDtoWithCharacters>>();

            try{
                var returnedSkills = await _context.Skills
                                        .Include(skill => skill.Characters)
                                        .ToListAsync();
                serviceResponse.Data = returnedSkills.Select(skill => _mapper.Map<GetSkillDtoWithCharacters>(skill)).ToList();
                return serviceResponse;

            }catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
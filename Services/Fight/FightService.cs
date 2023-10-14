using System.Net.Cache;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_api.Services.Fight
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService (DataContext context){
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request){
            var serviceResponse = new ServiceResponse<AttackResultDto>();
            try{
                var attacker = await _context.Characters
                                .Include(c => c.Weapon)
                                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if(attacker is null || opponent is null || attacker.Weapon is null){
                    throw new Exception($"This weapon fight cannot be done");
                }

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));

                if(damage > 0){
                    opponent.HitPoints -= damage;
                }

                if(opponent.HitPoints <= 0 ){
                    serviceResponse.Message = $"{opponent.Name} has been defeated !!!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto{
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request){
            var serviceResponse = new ServiceResponse<AttackResultDto>();
            try{
                var attacker = await _context.Characters
                                .Include(c => c.Skills)
                                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                                .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if(attacker is null || opponent is null || attacker.Skills is null){
                    throw new Exception($"This skill fight cannot be done");
                }

                var skill = attacker.Skills.FirstOrDefault(skill => skill.Id == request.SkillId); 

                if(skill is null){
                    throw new Exception($"The attacker do not have this skill ");
                }

                int damage = skill.Damage + (new Random().Next(attacker.Strength));

                if(damage > 0){
                    opponent.HitPoints -= damage;
                }

                if(opponent.HitPoints <= 0 ){
                    serviceResponse.Message = $"{opponent.Name} has been defeated !!!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto{
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
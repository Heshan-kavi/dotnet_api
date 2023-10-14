using System.Runtime.InteropServices;
using System.Text;
using System.Net.Http;
using System.Xml;
using System.Reflection.PortableExecutable;
using System.Net.Http.Headers;
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

        private static int DoWeaponAttack(Character attacker, Character opponent){
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defeats);

            if(damage > 0){
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill){
            int damage = skill.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defeats);

            if(damage > 0){
                opponent.HitPoints -= damage;
            }

            return damage;
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

                int damage = DoWeaponAttack(attacker, opponent);

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

                int damage = DoSkillAttack(attacker, opponent, skill);

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

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request){
            var serviceResponse = new ServiceResponse<FightResultDto>{
                Data = new FightResultDto()
            };

            try{
                bool defeated = false;
                var characters = await _context.Characters
                                    .Include(character => character.Weapon)
                                    .Include(character => character.Skills)
                                    .Where(character => request.CharacterIds.Contains(character.Id))
                                    .ToListAsync();

                while(!defeated){

                    foreach(var attacker in characters){
                        var opponents = characters.Where(character => character.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if(useWeapon && attacker.Weapon is not null){
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }else if(!useWeapon && attacker.Skills is not null){
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }else{
                            serviceResponse.Data.Log.Add($"{attacker.Name} wasn't able to attack at the moment !!");
                            continue;
                        }

                        serviceResponse.Data.Log.Add($"{attacker.Name} attacks using {attackUsed} with {(damage >= 0 ? damage : 0)}" );

                        if(opponent.HitPoints <= 0){
                        defeated = true;
                        opponent.Defeats++;
                        attacker.Victories++;
                        serviceResponse.Data.Log.Add($"{opponent.Name} has been defeated!");
                        serviceResponse.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left !");
                        break;
                        }
                    }
                }
                characters.ForEach(character => {
                    character.Fights++;
                    character.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
            }
            catch(Exception ex){
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
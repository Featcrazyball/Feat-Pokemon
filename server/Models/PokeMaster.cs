using System.ComponentModel.DataAnnotations;
using Database;
using Models;

namespace PokemonPocket
{
    public class PokemonMaster 
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? Type {get;set;}
        public int Level {get;set;}
        public int Experience {get;set;}

        // Feat's Features
        public float Health {get;set;}
        public float MaxHealth {get;set;}
        public float Attack {get;set;}
        public float SpecialAttack {get;set;}
        public float Defense { get;set; } 
        public float SpecialDefense {get;set;}
        public float Speed {get;set;}
        public string? OwnerId {get;set;}
        public int StatPoints {get;set;}
        public int? StatsEarned {get;set;}

        // IV's
        public int HpIV {get;set;}
        public int AttackIV {get;set;}
        public int SpecialAttackIV {get;set;}
        public int DefenseIV {get;set;}
        public int SpecialDefenseIV {get;set;}
        public int SpeedIV {get;set;}

        // Skills
        public virtual string? SkillPool { get; set; }
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

        // For Assignment
        public float SkillDamage { get;set; }
        public string? Skill {get;set;}

        public PokemonMaster() { } //For EF Core
        public PokemonMaster(string Name, string Type, float Health, float Attack, float Defense, float SpecialAttack, float SpecialDefense, float Speed, string OwnerId, int SkillDamage, string Skill) {
            Id = Guid.NewGuid().ToString("N")[..15]; 
            HpIV = Random.Shared.Next(1, 31);
            AttackIV = Random.Shared.Next(1, 31);
            SpecialAttackIV = Random.Shared.Next(1, 31);
            DefenseIV = Random.Shared.Next(1, 31);
            SpecialDefenseIV = Random.Shared.Next(1, 31);
            SpeedIV = Random.Shared.Next(1, 31);
            StatPoints = Random.Shared.Next(1, 10);
            Level = 1;
            Experience = 0;
            StatsEarned = 0;
            this.Name = Name;
            this.Type = Type;
            this.Health = Health;
            this.Attack = Attack;
            this.SpecialAttack = SpecialAttack;
            this.Defense = Defense;
            this.SpecialDefense = SpecialDefense;
            this.Speed = Speed;
            this.OwnerId = OwnerId;
            this.Skill = Skill;
            this.SkillDamage = SkillDamage;
            MaxHealth = Health;
        }

        // For Assignment
        public virtual float calculateDamage(float SkillDamage) {
            return SkillDamage;
        }

        // For Re-calculation DO NOT TOUCH
        public void EvolveLevelUp(int times)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 101) {
                    // Stat Upgrades
                    Level += 1;
                    Experience = 0;
                    Health += (Health + HpIV) / 50 + Level + 10;
                    Attack += (Attack + AttackIV) / 8 + 1; 
                    SpecialAttack += (SpecialAttack + SpecialAttackIV) / 8 + 1; 
                    Defense += (Defense + DefenseIV) / 8 + 1; 
                    SpecialDefense += (SpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    Speed += (Speed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                }
            }
            MaxHealth = Health;
        }

        // For Leveling Up
        public void LevelUp(int times)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 101) {
                    // Stat Upgrades
                    Level += 1;
                    Experience = 0;
                    Health += (Health + HpIV) / 50 + Level + 10;
                    Attack += (Attack + AttackIV) / 8 + 1; 
                    SpecialAttack += (SpecialAttack + SpecialAttackIV) / 8 + 1; 
                    Defense += (Defense + DefenseIV) / 8 + 1; 
                    SpecialDefense += (SpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    Speed += (Speed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                    Console.WriteLine($"Your {Name} has leveled up to level {Level}!\nYou have {StatPoints} Stat Points left.");

                    // Max Level Check
                    if (Level >= 100) { Console.WriteLine($"Your {Name} has reached a max level of 100!"); break;}
                }
            }
            MaxHealth = Health;
        }

        // Stat Points Management
        public void AddStatPoints(int points)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatsEarned < 251) { StatPoints += 1; StatsEarned -= 1; }
                else { break; }
            }
            Console.WriteLine($"Stat Points have been added to your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        public void RemoveStatPoints(int points)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatPoints > 0) { StatPoints -= 1;}
                else { break; }
            }
            Console.WriteLine($"Stat Points have been removed from your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        // Base Form Evolve (dont bother)
        public virtual void Evolve()
        {
            Console.WriteLine($"{Name} is unable to evolve.");
        }

        public bool LearnSkill(string skillName)
        {
            using var context = new DatabaseContext();
            
            // Check if already knows 4 skills
            var currentSkillCount = context.Skills.Count(s => s.PokemonId == Id);
            if (currentSkillCount >= 4)
            {
                Console.WriteLine($"{Name} already knows 4 skills. It must forget one first.");
                return false;
            }
            
            // Find a skill template with this name
            var skillTemplate = context.Skills.FirstOrDefault(s => s.Name == skillName && s.PokemonId == null);
            if (skillTemplate == null)
            {
                Console.WriteLine($"{skillName} does not exist.");
                return false;
            }
            
            // Create skill
            Skill newSkill;
            switch(skillName)
            {
                case "Absorb":
                    if (Id == null) { Console.WriteLine("Error registering Pokemon ID\nPlease contact an admin (just kidding theres no admin, call Featcrazyball)."); return false; }
                    newSkill = new Absorb(Id);
                    break;
                default:
                    Console.WriteLine($"Skill {skillName} not recognized.");
                    return false;
            }
            
            // Add to database
            context.Skills.Add(newSkill);
            context.SaveChanges();
            
            Console.WriteLine($"{Name} learned {skillName}!");
            return true;
        }


    }
}
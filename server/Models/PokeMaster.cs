using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arena;
using Database;
using Models;
using Server;

namespace PokemonPocket
{
    public class PokemonMaster 
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? Nickname {get;set;}
        public string? Type {get;set;}
        public int Level {get;set;}
        public int Experience {get;set;}

        // Feat's Features
        [NotMapped] public float Health {get;set;}
        [NotMapped] public float Attack {get;set;}
        [NotMapped] public float SpecialAttack {get;set;}
        [NotMapped] public float Defense { get;set; } 
        [NotMapped] public float SpecialDefense {get;set;}
        [NotMapped] public float Speed {get;set;}

        public float MaxHealth {get;set;}
        public float MaxAttack {get;set;}
        public float MaxSpecialAttack {get;set;}
        public float MaxDefense { get;set; }
        public float MaxSpecialDefense {get;set;}
        public float MaxSpeed {get;set;}

        [NotMapped] public int AttackStage {get;set;} = 0;
        [NotMapped] public int SpecialAttackStage {get;set;} = 0;
        [NotMapped] public int DefenseStage {get;set;} = 0;
        [NotMapped] public int SpecialDefenseStage {get;set;} = 0;
        [NotMapped] public int SpeedStage {get;set;} = 0;

        [NotMapped] public float CritRate {get;set;} = 1/16f; // Crit Rate
        [NotMapped] public float CritDmg {get;set;} = 1.5f; // Crit Damage

        // Extra Info
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

        // Owner
        public string? OwnerId {get;set;}
        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        // For Arena
        public bool Selected {get;set;}
        public bool Starter {get;set;} = false;

        // Arena 
        [NotMapped] public float BideDamage { get; set; } = 0; 
        [NotMapped] public int BideTurns { get; set; } = 0;
        [NotMapped] public bool BideActive { get; set; } = false;

        [NotMapped] public float BindDamage { get; set; } = 0;
        [NotMapped] public int BindTurns { get; set; } = 0;
        [NotMapped] public bool BindActive { get; set; } = false;

        [NotMapped] public bool Flinch { get; set; } = false;
        [NotMapped] public int FlinchTurns { get; set; } = 0;

        [NotMapped] public bool Paralyzed { get; set; } = false;
        [NotMapped] public bool ParalyzeSpeed { get; set; } = false;

        [NotMapped] public bool Freezing { get; set; } = false;

        // For Assignment
        public float SkillDamage { get;set; }
        public string? Skill {get;set;}

        public PokemonMaster() { } //For EF Core
        public PokemonMaster(string Name, string Type, float MaxHealth, float MaxAttack, float MaxDefense, float MaxSpecialAttack, float MaxSpecialDefense, float MaxSpeed, string OwnerId, int SkillDamage, string Skill) {
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
            this.MaxHealth = MaxHealth;
            this.MaxAttack = MaxAttack;
            this.MaxSpecialAttack = MaxSpecialAttack;
            this.MaxDefense = MaxDefense;
            this.MaxSpecialDefense = MaxSpecialDefense;
            this.MaxSpeed = MaxSpeed;
            this.OwnerId = OwnerId;
            this.Skill = Skill;
            this.SkillDamage = SkillDamage;
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;
        }

        // For Assignment
        public virtual float calculateDamage(float SkillDamage) {
            return SkillDamage;
        }

        public void ResetStats()
        {
            // Basic Stats
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;

            AttackStage = 0;
            SpecialAttackStage = 0;
            DefenseStage = 0;
            SpecialDefenseStage = 0;
            SpeedStage = 0;

            CritRate = 1 / 16f; // Crit Rate
            CritDmg = 1.5f; // Crit Damage

            // Extra Info
            BideDamage = 0;
            BideTurns = 0;
            BideActive = false;

            BindDamage = 0;
            BindTurns = 0;
            BindActive = false;

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
                    MaxHealth += (MaxHealth + HpIV) / 50 + Level + 10;
                    MaxAttack += (MaxAttack + AttackIV) / 8 + 1; 
                    MaxSpecialAttack += (MaxSpecialAttack + SpecialAttackIV) / 8 + 1; 
                    MaxDefense += (MaxDefense + DefenseIV) / 8 + 1; 
                    MaxSpecialDefense += (MaxSpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    MaxSpeed += (MaxSpeed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                }
            }
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;
        }

        // For Leveling Up
        public async Task LevelUp(int times, ClientSession session)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 101) {
                    // Stat Upgrades
                    Level += 1;
                    Experience = 0;
                    MaxHealth += (MaxHealth + HpIV) / 50 + Level + 10;
                    MaxAttack += (MaxAttack + AttackIV) / 8 + 1; 
                    MaxSpecialAttack += (MaxSpecialAttack + SpecialAttackIV) / 8 + 1; 
                    MaxDefense += (MaxDefense + DefenseIV) / 8 + 1; 
                    MaxSpecialDefense += (MaxSpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    MaxSpeed += (MaxSpeed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                    await session.SendMessageAsync($"Your {Name} has leveled up to level {Level}!\nYou have {StatPoints} Stat Points left.");

                    // Max Level Check
                    if (Level >= 100) { await session.SendMessageAsync($"Your {Name} has reached a max level of 100!"); break;}
                }
            }
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;
        }

        // Stat Points Management
        public async Task AddStatPoints(int points, ClientSession session)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatsEarned < 251) { StatPoints += 1; StatsEarned -= 1; }
                else { break; }
            }
            await session.SendMessageAsync($"Stat Points have been added to your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        public async Task RemoveStatPoints(int points, ClientSession session)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatPoints > 0) { StatPoints -= 1;}
                else { break; }
            }
            await session.SendMessageAsync($"Stat Points have been removed from your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        // Base Form Evolve (dont bother)
        public virtual async Task Evolve(ClientSession session)
        {
            await session.SendMessageAsync($"{Nickname} is unable evolve.");
        }

        // Skill Management (Incomplete)
        public async Task<bool> LearnSkill(string skillName, ClientSession session)
        {
            using var context = new DatabaseContext();
            
            // Check if already knows 4 skills
            var currentSkillCount = context.Skills.Count(s => s.PokemonId == Id);
            if (currentSkillCount >= 4)
            {
                await session.SendMessageAsync($"{Nickname} already knows 4 skills. It must forget one first.");
                return false;
            }
            
            // Find a skill template with this name
            var skillTemplate = context.Skills.FirstOrDefault(s => s.Name == skillName && s.PokemonId == null);
            if (skillTemplate == null)
            {
                await session.SendMessageAsync($"{skillName} does not exist.");
                return false;
            }
            
            // Create skill
            Skill newSkill;
            switch(skillName)
            {
                case "Absorb":
                    if (Id == null) { await session.SendMessageAsync("Error registering Pokemon ID\nPlease contact an admin (just kidding theres no admin, call Featcrazyball)."); return false; }
                    newSkill = new Absorb(Id);
                    break;
                default:
                    await session.SendMessageAsync($"Skill {skillName} is not recognized.");
                    return false;
            }
            
            // Add to database
            context.Skills.Add(newSkill);
            context.SaveChanges();
            
            await session.SendMessageAsync($"{Nickname} has learned {skillName}!");
            return true;
        }

        public async Task ForgetSkill(string skillName, ClientSession session)
        {
            using var context = new DatabaseContext();
            
            // Find the skill
            var skill = context.Skills.FirstOrDefault(s => s.Name == skillName && s.PokemonId == Id);
            if (skill == null)
            {
                await session.SendMessageAsync($"{Name} does not know {skillName}.");
                return;
            }
            
            // Remove from database
            context.Skills.Remove(skill);
            context.SaveChanges();
            
            Console.WriteLine($"{Name} has forgotten {skillName}!");
        }
    
        // Skill Pool Management (Incomplete)
        public virtual List<Skill>? LearnSkillFromSkillPool()
        {
            if (string.IsNullOrEmpty(SkillPool))
            {
                return null;
            }

            var skillNames = SkillPool.Split(',');

            var random = new Random();
            int numberOfSkillsToLearn = random.Next(1, Math.Min(4, skillNames.Length) + 1);
            var selectedSkillNames = new HashSet<string>(); 
            
            while (selectedSkillNames.Count < numberOfSkillsToLearn)
            {
                int randomIndex = random.Next(0, skillNames.Length);
                selectedSkillNames.Add(skillNames[randomIndex].Trim());
            }

            var newSkills = new List<Skill>();
            using var context = new DatabaseContext();
            
            foreach (var skillName in selectedSkillNames)
            {
                Skill? newSkill = null;
                
                switch(skillName)
                {
                    case "Absorb":
                        if (Id == null) continue;
                        newSkill = new Absorb(Id);
                        break;
                        
                    // Add cases for other skills here as you implement them
                    // case "Tackle":
                    //     newSkill = new Tackle(Id);
                    //     break;
                        
                    default:
                        break;
                }
                
                if (newSkill != null)
                {
                    context.Skills.Add(newSkill);
                    newSkills.Add(newSkill);
                } 
            }
            
            if (newSkills.Count > 0) {context.SaveChanges();}
                
            return newSkills.Count > 0 ? newSkills : null;
        }

        public void SetStarter(PokemonMaster poke) 
        {
            using var context = new DatabaseContext();
            foreach (var pokemon in poke.Owner?.Pokemon ?? new List<PokemonMaster>())
            {
                if (pokemon.Starter) { pokemon.Starter = false; }
            }
            poke.Starter = true;
            context.SaveChanges();
        }
    }
}
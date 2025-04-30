using System.ComponentModel.DataAnnotations;
using Models;

// Rememebr to add Item req for evolution
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
        public float Health {get;set;}
        public float Attack {get;set;}
        public float SpecialAttack {get;set;}
        public float Defense { get;set; } 
        public float SpecialDefense {get;set;}
        public float Speed {get;set;}
        public string? OwnerId {get;set;}
        public int StatPoints {get;set;}
        public int? StatsEarned {get;set;}

        public int HpIV {get;set;}
        public int AttackIV {get;set;}
        public int SpecialAttackIV {get;set;}
        public int DefenseIV {get;set;}
        public int SpecialDefenseIV {get;set;}
        public int SpeedIV {get;set;}

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
        }

        public virtual float calculateDamage(float SkillDamage) {
            return SkillDamage;
        }

        public void LevelUp(int times)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 100) {
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
        }

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

        // Remember to add items
        public string Evolve(string pokemon, string nickname)
        {
            switch (pokemon)
            {
                case "Bulbasaur" when Level >= 16:
                    Name = "Ivysaur";
                    Type = "Grass/Poison";

                    Health = 60;
                    Attack = 62;
                    SpecialAttack = 80;
                    Defense = 63;
                    SpecialDefense = 80;
                    Speed = 60;
                    StatPoints = 10;

                    LevelUp(Level); 
                    return $"{nickname} has evolved from a Bulbasaur to Ivysaur";

                case "Ivysaur" when Level >= 32:
                    Name = "Venusaur";
                    Type = "Grass/Poison";

                    Health = 80;
                    Attack = 82;
                    SpecialAttack = 100;
                    Defense = 83;
                    SpecialDefense = 100;
                    Speed = 80;
                    StatPoints = 10;

                    LevelUp(Level);
                    return $"{nickname} has evolved from a Ivysaur to Venusaur";

                default:
                    return $"{nickname} has failed to evolve.";
            }
        }
    }
    
}
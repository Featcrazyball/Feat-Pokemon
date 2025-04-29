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
        public int IV {get;set;}

        // For Assignment
        public float SkillDamage { get;set; }
        public string? Skill {get;set;}

        public PokemonMaster() { } //For EF Core
        public PokemonMaster(string Name, string Type, float Health, float Attack, float Defense, float SpecialAttack, float SpecialDefense, float Speed, string OwnerId, int SkillDamage, string Skill) {
            Id = Guid.NewGuid().ToString("N")[..15]; 
            IV = Random.Shared.Next(1, 31);
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

        public void LevelUp()
        {
            if (Experience > Level * 1000) {
                    Level += 1;
                    Experience = 0;
                    Health += (Health + IV) / 50 + Level + 10;
                    Attack += (Attack + IV) / 8 + 1; 
                    SpecialAttack += (SpecialAttack + IV) / 8 + 1; 
                    Defense += (Defense + IV) / 8 + 1; 
                    SpecialDefense += (SpecialDefense + IV) / 8 + 1; 
                    Speed += (Speed + IV) / 8 + 1;
                    if (StatsEarned < 251) {StatPoints += 3; StatsEarned += 3;}
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

                    for (int i = 0; i < Level; i++) { LevelUp(); }
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

                    for (int i = 0; i < Level; i++) { LevelUp(); }
                    return $"{nickname} has evolved from a Ivysaur to Venusaur";

                default:
                    return $"{nickname} has failed to evolve.";
            }
        }
    }
    
}
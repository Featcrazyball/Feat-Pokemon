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
        public int Health {get;set;}
        public int Attack {get;set;}
        public int SpecialAttack {get;set;}
        public int Defense { get;set; } 
        public int SpecialDefense {get;set;}
        public int Speed {get;set;}
        public string? OwnerId {get;set;}
        public int StatPoints {get;set;}
        public int IV {get;set;}

        public PokemonMaster() { } //For EF Core
        public PokemonMaster(string Name, string Type, int Level, int Experience, int Health, int Attack, int SpecialAttack, int Defense, int SpecialDefense, int Speed, string OwnerId, int StatPoints, int IV) {
            Id = Id; 
            this.Name = Name;
            this.Type = Type;
            this.Level = Level;
            this.Experience = Experience;
            this.Health = Health;
            this.Attack = Attack;
            this.SpecialAttack = SpecialAttack;
            this.Defense = Defense;
            this.SpecialDefense = SpecialDefense;
            this.Speed = Speed;
            this.OwnerId = OwnerId;
            this.StatPoints = StatPoints;
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
                    StatPoints += 3;
            }
        }

        public void EvolveToIvySaur()
        {
            if (Level >= 16) {
                Name = "Ivysaur";
                Type = "Grass/Poison";

                Health = 60;
                Attack = 62;
                SpecialAttack = 80;
                Defense = 63;
                SpecialDefense = 80;
                Speed = 60;
                StatPoints = 10;

                for (int i = 0; i < Level; i++) {
                    LevelUp();
                }
            }
        }

        public void EvolveToVenusaur()
        {
            if (Level >= 32) {
                Name = "Venusaur";
                Type = "Grass/Poison";

                Health = 80;
                Attack = 82;
                SpecialAttack = 100;
                Defense = 83;
                SpecialDefense = 100;
                Speed = 80;
                StatPoints = 10;

                for (int i = 0; i < Level; i++) {
                    LevelUp();
                }
            }
        }
    }
    
}
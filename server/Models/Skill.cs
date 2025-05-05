using System;
using PokemonPocket;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Skill
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } // ("Status", "Attack", "Special Attack", "Buff", "Debuff")
        public int BasePower { get; set; }
        public float Accuracy { get; set; }
        public int LevelRequired { get; set; } // Level required to use the skill
        public int PowerPoints { get; set; } // Number of times the skill can be used per Battle
        public int Cooldown { get; set; } // Number of turns before the skill can be used again
        public int EffectDuration { get; set; } // Duration of the effect in turns
        public string? Description { get; set; } // Description of the skill

        public bool InUse { get; set; } 

        // Link to Pokemon
        public string? PokemonId { get; set; }
        [ForeignKey("PokemonId")]
        public virtual PokemonMaster? Pokemon { get; set; } 

        protected Skill() {} //For EF Core
        public Skill(string Name, string Type, int BasePower, float Accuracy, int LevelRequired, int PowerPoints, int Cooldown, int EffectDuration, string Description, string PokemonId)
        {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15);
            this.Name = Name;
            this.Type = Type;
            this.BasePower = BasePower;
            this.Accuracy = Accuracy;
            this.LevelRequired = LevelRequired;
            this.PowerPoints = PowerPoints;
            this.Cooldown = Cooldown;
            this.EffectDuration = EffectDuration;
            this.Description = Description;
            this.PokemonId = PokemonId;
            InUse = false;
        }

        public virtual void SkillEfect(PokemonMaster target, PokemonMaster user, float Modifer) {
            Console.WriteLine($"The skill {Name} has no effect.");
        }
    }
}


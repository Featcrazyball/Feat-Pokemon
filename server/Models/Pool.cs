using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SkillPool
    {
        [Key]
        public string? Id { get; set; }
        public string? PokemonType { get; set; } // Name of Pokemon 
        public string? Name { get; set; } 
        public virtual ICollection<Skill> Skills { get; set; }
        
        // for EF Core
        private SkillPool() { Skills = new List<Skill>(); }
        public SkillPool(string pokemonType, string name)
        {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15);
            PokemonType = pokemonType;
            Name = name;
            Skills = new List<Skill>();
        }
        
        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);
        }
        
        public List<Skill> GetRandomSkills(int count)
        {
            if (Skills.Count <= count) { return Skills.ToList(); }
                
            var result = new List<Skill>();
            var availableSkills = Skills.ToList();
            var random = new Random();
            
            for (int i = 0; i < count && availableSkills.Count > 0; i++)
            {
                var index = random.Next(availableSkills.Count);
                result.Add(availableSkills[index]);
                availableSkills.RemoveAt(index);
            }
            
            return result;
        }
        
        public List<Skill> GetSkillsByLevel(int pokemonLevel)
        {
            return Skills.Where(s => s.LevelRequired <= pokemonLevel).ToList();
        }
    }
}
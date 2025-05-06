using System.ComponentModel.DataAnnotations;
using PokemonPocket;

namespace Models
{
    public class User
    {
        [Key]
        public string? Id { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Coins { get; set; }
        public bool FeatVersion { get; set; }

        // Link to Pokemon
        public virtual ICollection<PokemonMaster> Pokemon { get; set; } = new List<PokemonMaster>();
        public virtual ICollection<PokemonMaster> BattlePokemon { get; set; } = new List<PokemonMaster>();

        // Parameterless constructor for EF Core
        private User() { }

        // Instance constructor
        public User(string Username, string Password, string Email)
        {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15);
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;
            Wins = 0;
            Losses = 0;
            Coins = 0;
            FeatVersion = false;

        }

        public double CalculateWinLossRatio()
        {
            if (Losses == 0) return Wins; 
            return Math.Round((double)Wins / Losses, 2);
        }

        public void UpdateBattlePokemon()
        {
            BattlePokemon.Clear();
            
            foreach (var pm in Pokemon)
            {
                if (pm.Selected) {BattlePokemon.Add(pm);}
            }
        }
    }
}
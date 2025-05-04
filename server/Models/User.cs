using System.ComponentModel.DataAnnotations;
using System.Dynamic;

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
    }
}
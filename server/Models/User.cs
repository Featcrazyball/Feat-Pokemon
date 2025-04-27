using System.ComponentModel.DataAnnotations;

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

        // Parameterless constructor for EF Core
        private User() { }

        // Instance constructor
        public User(string Id, string Username, string Password, string Email, int Wins, int Losses, int Coins)
        {
            this.Id = Guid.NewGuid().ToString("N").Substring(0, 15);
            this.Username = Username;
            this.Password = Password;
            this.Email = Email;
            this.Wins = Wins;
            this.Losses = Losses;
            this.Coins = Coins;
        }

        public double CalculateWinLossRatio()
        {
            if (Losses == 0) return Wins; 
            return Math.Round((double)Wins / Losses, 2);
        }
    }
}
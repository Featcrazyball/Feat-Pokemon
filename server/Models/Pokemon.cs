namespace PokemonPocket
{
    public class Bulbasaur : PokemonMaster
    {
        public string? Nickname {get;set;}

        private Bulbasaur() { } //For EF Core
        public Bulbasaur(string Nickname, string Name, string Type, int Level, int Experience, int Health, int Attack, int SpecialAttack, int Defense, int SpecialDefense, int Speed, string OwnerId, int StatPoints, int IV) 
        : base(Name, Type, Level, Experience, Health, Attack, SpecialAttack, Defense, SpecialDefense, Speed, OwnerId, StatPoints, IV)
        {
            Id = Guid.NewGuid().ToString("N").Substring(0, 15);
            this.Nickname = Nickname;
            this.Name = "Bulbasaur";
            this.Type = "Grass/Poison";
            this.Level = Level;
            this.Experience = Experience;
            this.Health = 45;
            this.Attack = 49;
            this.SpecialAttack = 65;
            this.Defense = 49;
            this.SpecialDefense = 64;
            this.Speed = 45;
            this.OwnerId = OwnerId;
            this.StatPoints = Random.Shared.Next(1, 10);            
            this.IV = Random.Shared.Next(1, 31);
        }

        // Ask Teacher
        public int calculateDamage(int OpponentDefense) {
            return Attack * 2 / (OpponentDefense + 2) + 2;
        }
    }
}
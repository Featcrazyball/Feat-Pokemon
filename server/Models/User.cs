using System.ComponentModel.DataAnnotations;
using PokemonPocket;
using Server;
using Database;

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
        public bool God { get; set; } = false;

        public bool FreePika { get; set; } = false;
        public bool FreeEevee { get; set; } = false;
        public bool FreeChar { get; set; } = false;

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
            Coins = 100;

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

        public void GetPokemon(string name)
        {
            using (var context = new DatabaseContext())
            {
                PokemonMaster? pokemon = name.ToLower() switch
                {
                    "abra" => new Abra("None", Id!), "aerodactyl" => new Aerodactyl("None", Id!), "alakazam" => new Alakazam("None", Id!),
                    "arbok" => new Arbok("None", Id!), "arcanine" => new Arcanine("None", Id!), "articuno" => new Articuno("None", Id!),
                    "beedrill" => new Beedrill("None", Id!), "bellsprout" => new Bellsprout("None", Id!), "blastoise" => new Blastoise("None", Id!),
                    "bulbasaur" => new Bulbasaur("None", Id!), "butterfree" => new Butterfree("None", Id!), "caterpie" => new Caterpie("None", Id!),
                    "chansey" => new Chansey("None", Id!), "charizard" => new Charizard("None", Id!), "charmeleon" => new Charmeleon("None", Id!),
                    "charmander" => new Charmander("None", Id!), "clefable" => new Clefable("None", Id!), "clefairy" => new Clefairy("None", Id!),
                    "cloyster" => new Cloyster("None", Id!), "cubone" => new Cubone("None", Id!), "dewgong" => new Dewgong("None", Id!),
                    "diglett" => new Diglett("None", Id!), "ditto" => new Ditto("None", Id!), "dodrio" => new Dodrio("None", Id!),
                    "doduo" => new Doduo("None", Id!), "dragonair" => new Dragonair("None", Id!), "dragonite" => new Dragonite("None", Id!),
                    "dratini" => new Dratini("None", Id!), "drowzee" => new Drowzee("None", Id!), "dugtrio" => new Dugtrio("None", Id!),
                    "eevee" => new Eevee("None", Id!), "ekans" => new Ekans("None", Id!), "electabuzz" => new Electabuzz("None", Id!),
                    "electrode" => new Electrode("None", Id!), "exeggcute" => new Exeggcute("None", Id!), "exeggutor" => new Exeggutor("None", Id!),
                    "farfetch" => new Farfetch("None", Id!), "fearow" => new Fearow("None", Id!), "flareon" => new Flareon("None", Id!),
                    "gastly" => new Gastly("None", Id!), "gengar" => new Gengar("None", Id!), "geodude" => new Geodude("None", Id!),
                    "gloom" => new Gloom("None", Id!), "golbat" => new Golbat("None", Id!), "goldeen" => new Goldeen("None", Id!),
                    "golduck" => new Golduck("None", Id!), "golem" => new Golem("None", Id!), "graveler" => new Graveler("None", Id!),
                    "grimer" => new Grimer("None", Id!), "growlithe" => new Growlithe("None", Id!), "gyarados" => new Gyarados("None", Id!),
                    "haunter" => new Haunter("None", Id!), "hitmonchan" => new Hitmonchan("None", Id!), "hitmonlee" => new Hitmonlee("None", Id!),
                    "horsea" => new Horsea("None", Id!), "hypno" => new Hypno("None", Id!), "ivysaur" => new Ivysaur("None", Id!),
                    "jigglypuff" => new Jigglypuff("None", Id!), "jolteon" => new Jolteon("None", Id!), "jynx" => new Jynx("None", Id!),
                    "kabuto" => new Kabuto("None", Id!), "kabutops" => new Kabutops("None", Id!), "kadabra" => new Kadabra("None", Id!),
                    "kakuna" => new Kakuna("None", Id!), "kangaskhan" => new Kangaskhan("None", Id!), "kingler" => new Kingler("None", Id!),
                    "koffing" => new Koffing("None", Id!), "krabby" => new Krabby("None", Id!), "lapras" => new Lapras("None", Id!),
                    "lickitung" => new Lickitung("None", Id!), "machamp" => new Machamp("None", Id!), "machoke" => new Machoke("None", Id!),
                    "machop" => new Machop("None", Id!), "magikarp" => new Magikarp("None", Id!), "magmar" => new Magmar("None", Id!),
                    "magnemite" => new Magnemite("None", Id!), "magneton" => new Magneton("None", Id!), "mankey" => new Mankey("None", Id!),
                    "marowak" => new Marowak("None", Id!), "meowth" => new Meowth("None", Id!), "metapod" => new Metapod("None", Id!),
                    "mew" => new Mew("None", Id!), "mewtwo" => new Mewtwo("None", Id!), "moltres" => new Moltres("None", Id!),
                    "mr mime" => new MrMime("None", Id!), "muk" => new Muk("None", Id!), "nidoking" => new Nidoking("None", Id!),
                    "nidoqueen" => new Nidoqueen("None", Id!), "nidoranf" => new NidoranF("None", Id!), "nidoranm" => new NidoranM("None", Id!),
                    "nidorina" => new Nidorina("None", Id!), "nidorino" => new Nidorino("None", Id!), "ninetales" => new Ninetales("None", Id!),
                    "oddish" => new Oddish("None", Id!), "omanyte" => new Omanyte("None", Id!), "omastar" => new Omastar("None", Id!),
                    "onix" => new Onix("None", Id!), "paras" => new Paras("None", Id!), "parasect" => new Parasect("None", Id!),
                    "persian" => new Persian("None", Id!), "pidgeot" => new Pidgeot("None", Id!), "pidgeotto" => new Pidgeotto("None", Id!),
                    "pidgey" => new Pidgey("None", Id!), "pikachu" => new Pikachu("None", Id!), "pinsir" => new Pinsir("None", Id!),
                    "poliwag" => new Poliwag("None", Id!), "poliwhirl" => new Poliwhirl("None", Id!), "poliwrath" => new Poliwrath("None", Id!),
                    "ponyta" => new Ponyta("None", Id!), "porygon" => new Porygon("None", Id!), "primeape" => new Primeape("None", Id!),
                    "psyduck" => new Psyduck("None", Id!), "raichu" => new Raichu("None", Id!), "rapidash" => new Rapidash("None", Id!),
                    "raticate" => new Raticate("None", Id!), "rattata" => new Rattata("None", Id!), "rhydon" => new Rhydon("None", Id!),
                    "rhyhorn" => new Rhyhorn("None", Id!), "sandslash" => new Sandslash("None", Id!), "sandshrew" => new Sandshrew("None", Id!),
                    "scyther" => new Scyther("None", Id!), "seadra" => new Seadra("None", Id!), "seaking" => new Seaking("None", Id!),
                    "seel" => new Seel("None", Id!), "shellder" => new Shellder("None", Id!), "slowbro" => new Slowbro("None", Id!),
                    "slowpoke" => new Slowpoke("None", Id!), "snorlax" => new Snorlax("None", Id!), "spearow" => new Spearow("None", Id!),
                    "squirtle" => new Squirtle("None", Id!), "starmie" => new Starmie("None", Id!), "staryu" => new Staryu("None", Id!),
                    "tangela" => new Tangela("None", Id!), "tauros" => new Tauros("None", Id!), "tentacool" => new Tentacool("None", Id!),
                    "tentacruel" => new Tentacruel("None", Id!), "vaporeon" => new Vaporeon("None", Id!), "venomoth" => new Venomoth("None", Id!),
                    "venonat" => new Venonat("None", Id!), "venusaur" => new Venusaur("None", Id!), "victreebel" => new Victreebel("None", Id!),
                    "vileplume" => new Vileplume("None", Id!), "voltorb" => new Voltorb("None", Id!), "vulpix" => new Vulpix("None", Id!),
                    "wartortle" => new Wartortle("None", Id!), "weedle" => new Weedle("None", Id!), "weepinbell" => new Weepinbell("None", Id!),
                    "weezing" => new Weezing("None", Id!), "wigglytuff" => new Wigglytuff("None", Id!), "zapdos" => new Zapdos("None", Id!),
                    "zubat" => new Zubat("None", Id!),
                    _ => null
                };

                if (pokemon != null)
                {
                    Pokemon.Add(pokemon);
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Pokemon {name} not found.");
                }
            } 
        }

        // New method for registration process
        public PokemonMaster? GetPokemonWithoutSaving(string name, string userId)
        {
            PokemonMaster? pokemon = name.ToLower().Trim() switch
            {
                "abra" => new Abra("None", userId!), "aerodactyl" => new Aerodactyl("None", userId!), "alakazam" => new Alakazam("None", userId!),
                "arbok" => new Arbok("None", userId!), "arcanine" => new Arcanine("None", userId!), "articuno" => new Articuno("None", userId!),
                "beedrill" => new Beedrill("None", userId!), "bellsprout" => new Bellsprout("None", userId!), "blastoise" => new Blastoise("None", userId!),
                "bulbasaur" => new Bulbasaur("None", userId!), "butterfree" => new Butterfree("None", userId!), "caterpie" => new Caterpie("None", userId!),
                "chansey" => new Chansey("None", userId!), "charizard" => new Charizard("None", userId!), "charmeleon" => new Charmeleon("None", userId!),
                "charmander" => new Charmander("None", userId!), "clefable" => new Clefable("None", userId!), "clefairy" => new Clefairy("None", userId!),
                "cloyster" => new Cloyster("None", userId!), "cubone" => new Cubone("None",userId!), "dewgong" => new Dewgong("None", userId!),
                "diglett" => new Diglett("None", userId!), "ditto" => new Ditto("None", userId!), "dodrio" => new Dodrio("None", userId!),
                "doduo" => new Doduo("None", userId!), "dragonair" => new Dragonair("None", userId!), "dragonite" => new Dragonite("None", userId!),
                "dratini" => new Dratini("None", userId!), "drowzee" => new Drowzee("None", userId!), "dugtrio" => new Dugtrio("None", userId!),
                "eevee" => new Eevee("None", userId!), "ekans" => new Ekans("None", userId!), "electabuzz" => new Electabuzz("None", userId!),
                "electrode" => new Electrode("None", userId!), "exeggcute" => new Exeggcute("None", userId!), "exeggutor" => new Exeggutor("None", userId!),
                "farfetch" => new Farfetch("None", userId!), "fearow" => new Fearow("None", userId!), "flareon" => new Flareon("None", userId!),
                "gastly" => new Gastly("None", userId!), "gengar" => new Gengar("None", userId!), "geodude" => new Geodude("None", userId!),
                "gloom" => new Gloom("None", userId!), "golbat" => new Golbat("None", userId!), "goldeen" => new Goldeen("None", userId!),
                "golduck" => new Golduck("None", userId!), "golem" => new Golem("None", userId!), "graveler" => new Graveler("None", userId!),
                "grimer" => new Grimer("None", userId!), "growlithe" => new Growlithe("None", userId!), "gyarados" => new Gyarados("None", userId!),
                "haunter" => new Haunter("None", userId!), "hitmonchan" => new Hitmonchan("None", userId!), "hitmonlee" => new Hitmonlee("None", userId!),
                "horsea" => new Horsea("None", userId!), "hypno" => new Hypno("None", userId!), "ivysaur" => new Ivysaur("None", userId!),
                "jigglypuff" => new Jigglypuff("None", userId!), "jolteon" => new Jolteon("None", userId!), "jynx" => new Jynx("None", userId!),
                "kabuto" => new Kabuto("None", userId!), "kabutops" => new Kabutops("None", userId!), "kadabra" => new Kadabra("None", userId!),
                "kakuna" => new Kakuna("None", userId!), "kangaskhan" => new Kangaskhan("None", userId!), "kingler" => new Kingler("None", userId!),
                "koffing" => new Koffing("None", userId!), "krabby" => new Krabby("None", userId!), "lapras" => new Lapras("None", userId!),
                "lickitung" => new Lickitung("None", userId!), "machamp" => new Machamp("None", userId!), "machoke" => new Machoke("None", userId!),
                "machop" => new Machop("None", userId!), "magikarp" => new Magikarp("None", userId!), "magmar" => new Magmar("None", userId!),
                "magnemite" => new Magnemite("None", userId!), "magneton" => new Magneton("None", userId!), "mankey" => new Mankey("None", userId!),
                "marowak" => new Marowak("None", userId!), "meowth" => new Meowth("None", userId!), "metapod" => new Metapod("None", userId!),
                "mew" => new Mew("None", userId!), "mewtwo" => new Mewtwo("None", userId!), "moltres" => new Moltres("None", userId!),
                "mr mime" => new MrMime("None", userId!), "muk" => new Muk("None", userId!), "nidoking" => new Nidoking("None", userId!),
                "nidoqueen" => new Nidoqueen("None", userId!), "nidoranf" => new NidoranF("None", userId!), "nidoranm" => new NidoranM("None", userId!),
                "nidorina" => new Nidorina("None", userId!), "nidorino" => new Nidorino("None", userId!), "ninetales" => new Ninetales("None", userId!),
                "oddish" => new Oddish("None", userId!), "omanyte" => new Omanyte("None", userId!), "omastar" => new Omastar("None", userId!),
                "onix" => new Onix("None", userId!), "paras" => new Paras("None", userId!), "parasect" => new Parasect("None", userId!),
                "persian" => new Persian("None", userId!), "pidgeot" => new Pidgeot("None", userId!), "pidgeotto" => new Pidgeotto("None", userId!),
                "pidgey" => new Pidgey("None", userId!), "pikachu" => new Pikachu("None", userId!), "pinsir" => new Pinsir("None", userId!),
                "poliwag" => new Poliwag("None", userId!), "poliwhirl" => new Poliwhirl("None", userId!), "poliwrath" => new Poliwrath("None", userId!),
                "ponyta" => new Ponyta("None", userId!), "porygon" => new Porygon("None", userId!), "primeape" => new Primeape("None", userId!),
                "psyduck" => new Psyduck("None", userId!), "raichu" => new Raichu("None", userId!), "rapidash" => new Rapidash("None", userId!),
                "raticate" => new Raticate("None", userId!), "rattata" => new Rattata("None", userId!), "rhydon" => new Rhydon("None", userId!),
                "rhyhorn" => new Rhyhorn("None", userId!), "sandslash" => new Sandslash("None", userId!), "sandshrew" => new Sandshrew("None", userId!),
                "scyther" => new Scyther("None", userId!), "seadra" => new Seadra("None", userId!), "seaking" => new Seaking("None", userId!),
                "seel" => new Seel("None", userId!), "shellder" => new Shellder("None", userId!), "slowbro" => new Slowbro("None", userId!),
                "slowpoke" => new Slowpoke("None", userId!), "snorlax" => new Snorlax("None", userId!), "spearow" => new Spearow("None", userId!),
                "squirtle" => new Squirtle("None", userId!), "starmie" => new Starmie("None", userId!), "staryu" => new Staryu("None", userId!),
                "tangela" => new Tangela("None", userId!), "tauros" => new Tauros("None", userId!), "tentacool" => new Tentacool("None", userId!),
                "tentacruel" => new Tentacruel("None", userId!), "vaporeon" => new Vaporeon("None", userId!), "venomoth" => new Venomoth("None", userId!),
                "venonat" => new Venonat("None", userId!), "venusaur" => new Venusaur("None", userId!), "victreebel" => new Victreebel("None", userId!),
                "vileplume" => new Vileplume("None", userId!), "voltorb" => new Voltorb("None", userId!), "vulpix" => new Vulpix("None", userId!),
                "wartortle" => new Wartortle("None", userId!), "weedle" => new Weedle("None", userId!), "weepinbell" => new Weepinbell("None", userId!),
                "weezing" => new Weezing("None", userId!), "wigglytuff" => new Wigglytuff("None", userId!), "zapdos" => new Zapdos("None", userId!),
                "zubat" => new Zubat("None", userId!),
                _ => null
            };
            
            // Return the created Pokemon for batch saving later
            return pokemon;
        }
    }
}
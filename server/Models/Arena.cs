using Server;
using PokemonPocket;
using Models;

namespace Arena;

// Make it so that if Bide is still being used, lock it so cant use untill ends.
// Check if bind if in effect, if in effect, cannot change pokemon
// Check if flinch, cannopt move

// When a Pokémon is paralyzed, it has a chance to be unable to act during its turn. Specifically, there is a 25% chance that a paralyzed Pokémon will be unable to move and will lose its turn.


public class Arena
{
    public User? creator { get; set; }
    public User? joiner { get; set; }

    // Creator Pokemon
    public virtual ICollection<PokemonMaster>? creatorPokemon { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? CreatorBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? creatorFainted { get; set; } = new List<PokemonMaster>();

    // Joiner Pokemon
    public virtual ICollection<PokemonMaster>? joinerPokemon { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? JoinerBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? joinerFainted { get; set; } = new List<PokemonMaster>();

    // Battle Stats
    public int turn { get; set; } = 0;

    public Arena(User player1, User player2)
    {
        creator = player1;
        joiner = player2;

        // Creator Pokemon
        creatorPokemon = player1.Pokemon.ToList();
        creatorFainted = new List<PokemonMaster>();
        CreatorBattle = creatorPokemon.FirstOrDefault(p => p.Starter);

        // Joiner Pokemon
        joinerPokemon = player2.Pokemon.ToList();
        joinerFainted = new List<PokemonMaster>();
        JoinerBattle = joinerPokemon.FirstOrDefault(p => p.Starter);
    }

    // Very important for killing pokemon and ending batle
    public async Task CheckStats(ClientSession creator, ClientSession joiner)
    {
        if (creatorPokemon == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (joinerPokemon == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }
        if (CreatorBattle == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (JoinerBattle == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }
        if (creatorFainted == null) creatorFainted = new List<PokemonMaster>();
        if (joinerFainted == null) joinerFainted = new List<PokemonMaster>();

        foreach (var pokemon in creatorPokemon)
        {
            if (pokemon.Health > pokemon.MaxHealth) pokemon.Health = pokemon.MaxHealth;
            if (pokemon.Attack > pokemon.MaxAttack) pokemon.Attack = pokemon.MaxAttack;
            if (pokemon.Defense > pokemon.MaxDefense) pokemon.Defense = pokemon.MaxDefense;
            if (pokemon.SpecialAttack > pokemon.MaxSpecialAttack) pokemon.SpecialAttack = pokemon.MaxSpecialAttack;
            if (pokemon.SpecialDefense > pokemon.MaxSpecialDefense) pokemon.SpecialDefense = pokemon.MaxSpecialDefense;
            if (pokemon.Speed > pokemon.MaxSpeed) pokemon.Speed = pokemon.MaxSpeed;
        }

        foreach (var pokemon in joinerPokemon)
        {
            if (pokemon.Health > pokemon.MaxHealth) pokemon.Health = pokemon.MaxHealth;
            if (pokemon.Attack > pokemon.MaxAttack) pokemon.Attack = pokemon.MaxAttack;
            if (pokemon.Defense > pokemon.MaxDefense) pokemon.Defense = pokemon.MaxDefense;
            if (pokemon.SpecialAttack > pokemon.MaxSpecialAttack) pokemon.SpecialAttack = pokemon.MaxSpecialAttack;
            if (pokemon.SpecialDefense > pokemon.MaxSpecialDefense) pokemon.SpecialDefense = pokemon.MaxSpecialDefense;
            if (pokemon.Speed > pokemon.MaxSpeed) pokemon.Speed = pokemon.MaxSpeed;
        }
        
        // Check for fainted Pokemon
        if (CreatorBattle.Health <= 0)
        {
            creatorFainted.Add(CreatorBattle);
            creatorPokemon.Remove(CreatorBattle);
            CreatorBattle = creatorPokemon.FirstOrDefault(p => p.Starter);
            if (CreatorBattle == null) { await creator.SendMessageAsync("You have no Pokemon left!"); return; }
        }
        if (JoinerBattle.Health <= 0)
        {
            joinerFainted.Add(JoinerBattle);
            joinerPokemon.Remove(JoinerBattle);
            JoinerBattle = joinerPokemon.FirstOrDefault(p => p.Starter);
            if (JoinerBattle == null) { await joiner.SendMessageAsync("You have no Pokemon left!"); return; }
        }
    }

    public async Task StartTurn(ClientSession creator, ClientSession joiner)
    {
        // Change this
        if (creatorPokemon == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (joinerPokemon == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }
        if (CreatorBattle == null) { await creator.SendMessageAsync("You have no Pokemon!"); return; }
        if (JoinerBattle == null) { await joiner.SendMessageAsync("You have no Pokemon!"); return; }

        // Check for status effects

        // Check for Bide
        foreach (var pokemon in creatorPokemon)
            if (pokemon.BideActive && pokemon.BideTurns > 0)
            {
                pokemon.BideTurns--;
                JoinerBattle.Health -= pokemon.BideDamage * 2; // Apply Bide damage to the opponent

                if (pokemon.BideTurns == 0)
                {
                    pokemon.BideActive = false;
                    pokemon.BideDamage = 0;
                }
            }
        
        foreach (var pokemon in joinerPokemon)
            if (pokemon.BideActive && pokemon.BideTurns > 0)
            {
                pokemon.BideTurns--;
                CreatorBattle.Health -= pokemon.BideDamage * 2; // Apply Bide damage to the opponent

                if (pokemon.BideTurns == 0)
                {
                    pokemon.BideActive = false;
                    pokemon.BideDamage = 0;
                }
            }

        // Check for Bind
        foreach (var pokemon in creatorPokemon)
            if (pokemon.BindActive && pokemon.BindTurns > 0)
            {
                pokemon.BindTurns--;
                JoinerBattle.Health -= pokemon.BindDamage;

                if (pokemon.BindTurns == 0)
                {
                    pokemon.BindActive = false;
                    pokemon.BindDamage = 0;
                }
            }

        foreach (var pokemon in joinerPokemon)
            if (pokemon.BindActive && pokemon.BindTurns > 0)
            {
                pokemon.BindTurns--;
                CreatorBattle.Health -= pokemon.BindDamage;

                if (pokemon.BindTurns == 0)
                {
                    pokemon.BindActive = false;
                    pokemon.BindDamage = 0;
                }
            }



        turn++;
    }

    public void StartBattle()
    {
        // Reset Pokemon Stats for both players
        if (creatorPokemon == null || joinerPokemon == null) return;

        foreach (var pokemon in creatorPokemon) pokemon.ResetStats();
        foreach (var pokemon in joinerPokemon) pokemon.ResetStats();

        // Reset Skill Stats for both players

        if (creator != null && joiner != null)
        {
            Console.WriteLine($"Battle started between {creator.Username} and {joiner.Username}!");
        }
        else
        {
            Console.WriteLine("Battle cannot start because one or both players are missing.");
        }
    }

}
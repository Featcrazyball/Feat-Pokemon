using System;
using System.Collections.Generic;
using PokemonPocket;

namespace Arena;

public class PokemonStats
{
    public float MaxHealth { get; set; }
    public float MaxAttack { get; set; }
    public float MaxDefense { get; set; }
    public float MaxSpecialAttack { get; set; }
    public float MaxSpecialDefense { get; set; }
    public float MaxSpeed { get; set; }

    public string Type { get; set; } = string.Empty;
}

public class PokemonBackupService
{
    public Dictionary<string, PokemonStats> CreatorPokemonStats { get; private set; } = new Dictionary<string, PokemonStats>();
    public Dictionary<string, PokemonStats> JoinerPokemonStats { get; private set; } = new Dictionary<string, PokemonStats>();
    
    public void BackupPokemonStats(
        IEnumerable<PokemonMaster> creatorPokemon, 
        PokemonMaster creatorBattle,
        IEnumerable<PokemonMaster> joinerPokemon,
        PokemonMaster joinerBattle)
    {
        // Clear previous backups
        CreatorPokemonStats.Clear();
        JoinerPokemonStats.Clear();
        
        // Backup creator pokemon stats
        foreach (var pokemon in creatorPokemon)
        {
            CreatorPokemonStats[pokemon.Id!] = CreateStatsFrom(pokemon);
        }
        
        // Also backup the starting Pokemon if it exists
        if (creatorBattle != null)
        {
            CreatorPokemonStats[creatorBattle.Id!] = CreateStatsFrom(creatorBattle);
        }
        
        // Backup joiner pokemon stats
        foreach (var pokemon in joinerPokemon)
        {
            JoinerPokemonStats[pokemon.Id!] = CreateStatsFrom(pokemon);
        }
        
        // Also backup the starting Pokemon if it exists
        if (joinerBattle != null)
        {
            JoinerPokemonStats[joinerBattle.Id!] = CreateStatsFrom(joinerBattle);
        }
    }
    
    public void RestorePokemonStats(
        IEnumerable<PokemonMaster> creatorPokemon,
        PokemonMaster creatorBattle,
        IEnumerable<PokemonMaster> joinerPokemon,
        PokemonMaster joinerBattle,
        IEnumerable<PokemonMaster> creatorFainted,
        IEnumerable<PokemonMaster> joinerFainted)
    {
        // Restore creator pokemon stats
        foreach (var pokemon in creatorPokemon)
        {
            if (CreatorPokemonStats.TryGetValue(pokemon.Id!, out var stats))
            {
                RestoreStatsTo(pokemon, stats);
            }
        }
        
        // Also restore the creator's battle Pokemon if it exists
        if (creatorBattle != null && CreatorPokemonStats.TryGetValue(creatorBattle.Id!, out var creatorBattleStats))
        {
            RestoreStatsTo(creatorBattle, creatorBattleStats);
        }
        
        // Restore joiner pokemon stats
        foreach (var pokemon in joinerPokemon)
        {
            if (JoinerPokemonStats.TryGetValue(pokemon.Id!, out var stats))
            {
                RestoreStatsTo(pokemon, stats);
            }
        }
        
        // Also restore the joiner's battle Pokemon if it exists
        if (joinerBattle != null && JoinerPokemonStats.TryGetValue(joinerBattle.Id!, out var joinerBattleStats))
        {
            RestoreStatsTo(joinerBattle, joinerBattleStats);
        }
        
        // Restore fainted pokemon with half HP
        foreach (var pokemon in creatorFainted)
        {
            if (CreatorPokemonStats.TryGetValue(pokemon.Id!, out var stats))
            {
                RestoreStatsTo(pokemon, stats);
            }
        }
        
        foreach (var pokemon in joinerFainted)
        {
            if (JoinerPokemonStats.TryGetValue(pokemon.Id!, out var stats))
            {
                RestoreStatsTo(pokemon, stats);
            }
        }
    }
    
    public void ClearStatusConditions(
        IEnumerable<PokemonMaster> creatorPokemon,
        PokemonMaster creatorBattle,
        IEnumerable<PokemonMaster> joinerPokemon,
        PokemonMaster joinerBattle,
        IEnumerable<PokemonMaster> creatorFainted,
        IEnumerable<PokemonMaster> joinerFainted)
    {
        // Create a list of all Pokemon to process
        var allCreatorPokemon = new List<PokemonMaster>();
        allCreatorPokemon.AddRange(creatorPokemon);
        if (creatorBattle != null) allCreatorPokemon.Add(creatorBattle);
        allCreatorPokemon.AddRange(creatorFainted);
        
        var allJoinerPokemon = new List<PokemonMaster>();
        allJoinerPokemon.AddRange(joinerPokemon);
        if (joinerBattle != null) allJoinerPokemon.Add(joinerBattle);
        allJoinerPokemon.AddRange(joinerFainted);
        
        // Clear status conditions for all Pokemon
        foreach (var pokemon in allCreatorPokemon.Where(p => p != null))
        {
            ClearStatusConditions(pokemon);
        }
        
        foreach (var pokemon in allJoinerPokemon.Where(p => p != null))
        {
            ClearStatusConditions(pokemon);
        }
    }
    
    private PokemonStats CreateStatsFrom(PokemonMaster pokemon)
    {
        return new PokemonStats
        {
            MaxHealth = pokemon.MaxHealth,
            MaxAttack = pokemon.MaxAttack,
            MaxDefense = pokemon.MaxDefense,
            MaxSpecialAttack = pokemon.MaxSpecialAttack,
            MaxSpecialDefense = pokemon.MaxSpecialDefense,
            MaxSpeed = pokemon.MaxSpeed,
            Type = pokemon.Type ?? string.Empty
        };
    }

    private void RestoreStatsTo(PokemonMaster pokemon, PokemonStats stats)
    {
        // Restore max values first
        pokemon.MaxHealth = stats.MaxHealth;
        pokemon.MaxAttack = stats.MaxAttack;
        pokemon.MaxDefense = stats.MaxDefense;
        pokemon.MaxSpecialAttack = stats.MaxSpecialAttack;
        pokemon.MaxSpecialDefense = stats.MaxSpecialDefense;
        pokemon.MaxSpeed = stats.MaxSpeed;

        pokemon.Health = stats.MaxHealth;
        pokemon.Attack = stats.MaxAttack;
        pokemon.Defense = stats.MaxDefense;
        pokemon.SpecialAttack = stats.MaxSpecialAttack;
        pokemon.SpecialDefense = stats.MaxSpecialDefense;
        pokemon.Speed = stats.MaxSpeed;

        pokemon.Type = stats.Type;
    }
    
    private void ClearStatusConditions(PokemonMaster pokemon)
    {
        pokemon.Paralyzed = false;
        pokemon.Burning = false;
        pokemon.Poisoned = false;
        pokemon.BadlyPoisoned = false;
        pokemon.Sleeping = false;
        pokemon.Freezing = false;
        pokemon.Confused = false;
        pokemon.ParalyzeSpeed = false;
    }
}
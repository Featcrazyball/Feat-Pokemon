using System;
using System.Collections.Generic;
using PokemonPocket;

namespace Arena;

public class PokemonStats
{
    public float Health { get; set; }
    public float Attack { get; set; }
    public float Defense { get; set; }
    public float SpecialAttack { get; set; }
    public float SpecialDefense { get; set; }
    public float Speed { get; set; }

    public int AttackStage { get; set; }
    public int DefenseStage { get; set; }
    public int SpeedStage { get; set; }
    public int SpecialAttackStage { get; set; }
    public int SpecialDefenseStage { get; set; }
    public int AccuracyStage { get; set; }
    public int EvasionStage { get; set; }
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
                RestoreStatsTo(pokemon, stats, halfHealth: true);
            }
        }
        
        foreach (var pokemon in joinerFainted)
        {
            if (JoinerPokemonStats.TryGetValue(pokemon.Id!, out var stats))
            {
                RestoreStatsTo(pokemon, stats, halfHealth: true);
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
            Health = pokemon.Health,
            Attack = pokemon.Attack,
            Defense = pokemon.Defense,
            SpecialAttack = pokemon.SpecialAttack,
            SpecialDefense = pokemon.SpecialDefense,
            Speed = pokemon.Speed,
            AttackStage = pokemon.AttackStage,
            DefenseStage = pokemon.DefenseStage,
            SpeedStage = pokemon.SpeedStage,
            SpecialAttackStage = pokemon.SpecialAttackStage,
            SpecialDefenseStage = pokemon.SpecialDefenseStage,
            AccuracyStage = pokemon.AccuracyStage,
            EvasionStage = pokemon.EvasionStage
        };
    }
    
    private void RestoreStatsTo(PokemonMaster pokemon, PokemonStats stats, bool halfHealth = false)
    {
        pokemon.Health = halfHealth ? stats.Health / 2 : stats.Health;
        pokemon.Attack = stats.Attack;
        pokemon.Defense = stats.Defense;
        pokemon.SpecialAttack = stats.SpecialAttack;
        pokemon.SpecialDefense = stats.SpecialDefense;
        pokemon.Speed = stats.Speed;
        pokemon.AttackStage = stats.AttackStage;
        pokemon.DefenseStage = stats.DefenseStage;
        pokemon.SpeedStage = stats.SpeedStage;
        pokemon.SpecialAttackStage = stats.SpecialAttackStage;
        pokemon.SpecialDefenseStage = stats.SpecialDefenseStage;
        pokemon.AccuracyStage = stats.AccuracyStage;
        pokemon.EvasionStage = stats.EvasionStage;
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
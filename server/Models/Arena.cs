using Server;
using PokemonPocket;
using Models;

namespace Arena;

// Make it so that if Bide is still being used, lock it so cant use untill ends.
// Check if bind if in effect, if in effect, cannot change pokemon
// Check if flinch, cannopt move for THAT TURN (reset every turn)

// When a Pokémon is paralyzed, it has a chance to be unable to act during its turn. Specifically, there is a 25% chance that a paralyzed Pokémon will be unable to move and will lose its turn.
// When first turn, bind the skill used to pokemon for Conversion
// Reminder for Dig, it will not hit if the opponent is underground, so check if the opponent is underground before applying damage
// Reminder for Confusion, if the target is confused, it has a 50% chance to hit itself in confusion. If it does, it will take damage equal to 40% of its max health. The confusion lasts for 2-5 turns.
// Reminder for disable, it will disable the last move used by the target for 2-5 turns. The target will not be able to use that move during that time. If the target tries to use the disabled
// Reminder for dig to fix modifeier
// Reminder for burn, it will reduce the attack of the target by 50% and will deal damage equal to 1/16 of its max health at the end of each turn. Switching out does not solve burning
// Reminder for freozen, it will prevent the target from moving. The frozen status can be removed by using certain moves or items, or by switching out the Pokémon. The frozen status lasts until the target is thawed or switched out.
// Reminder for Fly, for their priority next turn
// Reminder to make a function to check attributes, cuz paralyze and burn reduce and stage attribute changes. this will occur after skill is used for both players
// Reminder for Hyper Beam, it will require a recharge turn unless it defeats the target. If the target is knocked out, the user will not need to recharge. The recharge turn will be skipped if the target is knocked out.
// Reminder for Sleep, it will prevent the target from moving. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Reminder for light screen to reduce turns and check if it is active
// Reminder for leech seed to check if it is active and reduce turns
// Reminder to lower Mist turns every turn
// Reminder for Mimic, can only be used once
// Reminder to delete metronome skill after battle
// Reminder to add coins from payday after battle
// Reminder for Petal Dance, it will hit 2-5 times in a row. The user will become confused after the last hit. The confusion lasts for 2-5 turns. The confusion can be removed by using certain moves or items, or by switching out the Pokémon. The confusion lasts until the target is healed or switched out.
// Reminder for Poison, it will reduce the health of the target by 1/16 of its max health at the end of each turn. Switching out does not solve poison
// Reminder for Rage, turn it off
// Reminder for sleep, confusion, paralysis, and freeze, they cure when swicthed out
// Reminder for razor wind, it charge on first, and then can attk on 2nd or 3rd or wtv
// Reminder for Rest, it will heal the user to full health and remove all status conditions. The user will fall asleep for 2 turns. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Rest clears burn, paralysis, freeze, and poison. It does not clear confusion or sleep. The sleep status can be removed by using certain moves or items, or by switching out the Pokémon. The sleep status lasts until the target is healed or switched out.
// Reminder sleep does not cure when switched out
// Reminder for roar to switch out pokemon if lower level
// Reminder for struggle to only be used if no pp left
// Reminder for thrash, it will hit 2-3 times in a row. The user will become confused after the last hit. The confusion lasts for 2-5 turns. The confusion can be removed by using certain moves or items, or by switching out the Pokémon. The confusion lasts until the target is healed or switched out.
// Reminder to undo conversion after battle to original type
// reminder for badly poisoned, the turns increase by 1 every turn. The damage is calculated as 1/16 of the target's max health + 1/16 of the target's max health * turns. The damage is applied at the end of each turn. The badly poisoned status can be removed by using certain moves or items, or by switching out the Pokémon. The badly poisoned status lasts until the target is healed or switched out.
// Reminder for transform to return everything to normal after battle
// Reminder for transform that it returns to original form after battle
// Reminder for transform to use skills of target
// Reminder for whrilwind to switch out pokemon

public class Arena
{
    public User? creator { get; set; }
    public User? joiner { get; set; }

    // Creator Pokemon
    public virtual ICollection<PokemonMaster>? creatorPokemon { get; set; } = new List<PokemonMaster>();
    public virtual ICollection<PokemonMaster>? CreatorBackup { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? CreatorBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? creatorFainted { get; set; } = new List<PokemonMaster>();

    // Joiner Pokemon
    public virtual ICollection<PokemonMaster>? joinerPokemon { get; set; } = new List<PokemonMaster>();
    public virtual ICollection<PokemonMaster>? JoinerBackup { get; set; } = new List<PokemonMaster>();
    public PokemonMaster? JoinerBattle { get; set; } = null;
    public virtual ICollection<PokemonMaster>? joinerFainted { get; set; } = new List<PokemonMaster>();

    // Backups
    public Dictionary<string, PokemonStats> JoinerPokemonStats { get; private set; } = new Dictionary<string, PokemonStats>();

    // Battle Stats
    public int turn { get; set; } = 0;

    private readonly PokemonBackupService _backupService = new PokemonBackupService();

    public Arena(User player1, User player2)
    {
        creator = player1;
        joiner = player2;

        // Creator Pokemon
        creatorPokemon = player1.Pokemon.Where(p => p.Selected && !p.Starter).ToList();
        creatorFainted = new List<PokemonMaster>();
        CreatorBattle = creatorPokemon.FirstOrDefault(p => p.Starter);

        // Joiner Pokemon
        joinerPokemon = player2.Pokemon.Where(p => p.Selected).ToList();
        joinerFainted = new List<PokemonMaster>();
        JoinerBattle = joinerPokemon.FirstOrDefault(p => p.Starter);

        // Backups
        CreatorBackup = creatorPokemon.ToList();
        JoinerBackup = joinerPokemon.ToList();
        
        // Create stat backups
        _backupService.BackupPokemonStats(
            creatorPokemon, 
            CreatorBattle!, 
            joinerPokemon, 
            JoinerBattle!);
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

        // Status effects and if skip turn
        // Confusion
        bool NoCreatorTurn = await SkillHelper.ProcessConfusion(CreatorBattle, creator, joiner);
        bool NoJoinerTurn = await SkillHelper.ProcessConfusion(JoinerBattle, joiner, creator);

        // Check for Bide
        if (CreatorBattle.BideActive && CreatorBattle.BideTurns > 0)
        {
            CreatorBattle.BideTurns--;

            if (CreatorBattle.BideTurns == 0)
            {
                CreatorBattle.BideActive = false;
                JoinerBattle.Health -= CreatorBattle.BideDamage * 2; // Apply Bide damage to the opponent
                CreatorBattle.BideDamage = 0;
            }
            NoCreatorTurn = true; 
        }

        if (JoinerBattle.BideActive && JoinerBattle.BideTurns > 0)
        {
            JoinerBattle.BideTurns--;

            if (JoinerBattle.BideTurns == 0)
            {
                JoinerBattle.BideActive = false;
                CreatorBattle.Health -= JoinerBattle.BideDamage * 2; // Apply Bide damage to the opponent
                JoinerBattle.BideDamage = 0;
            }
            NoJoinerTurn = true; 
        }
        
        // Check for Bind
        if (CreatorBattle.BindActive && CreatorBattle.BindTurns > 0)
        {
            CreatorBattle.BindTurns--;
            CreatorBattle.Health -= CreatorBattle.BindDamage;

            if (CreatorBattle.BindTurns == 0)
            {
                CreatorBattle.BindActive = false;
                CreatorBattle.BindDamage = 0;
            }
            NoCreatorTurn = true;
        }

        if (JoinerBattle.BindActive && JoinerBattle.BindTurns > 0)
        {
            JoinerBattle.BindTurns--;
            JoinerBattle.Health -= JoinerBattle.BindDamage;

            if (JoinerBattle.BindTurns == 0)
            {
                JoinerBattle.BindActive = false;
                JoinerBattle.BindDamage = 0;
            }
            NoJoinerTurn = true;
        }

        // Dig
        if (CreatorBattle.Dig && NoCreatorTurn == false) {
            NoCreatorTurn = true;
            await SkillHelper.ProcessDig(JoinerBattle, CreatorBattle, creator, joiner);
        }  

        if (JoinerBattle.Dig && NoJoinerTurn == false) {
            NoJoinerTurn = true;
            await SkillHelper.ProcessDig(CreatorBattle, JoinerBattle, joiner, creator);
        }  

        // Skill
        string Creatorskill;
        string Joinerskill;

        // Ask for skill
        if (!NoCreatorTurn)
        {
            await creator.SendMessageAsync("Unfortunately, you cannot use your skill this turn. Please wait for your opponent to finish their turn.");
        } else
        {
            for (int i = 0; i < CreatorBattle.Skills.Count; i++)
            {
                var skill = CreatorBattle.Skills.ElementAt(i);
                await creator.SendMessageAsync($"{i + 1}. {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
            }
            Creatorskill = await creator.GetInputAsync($"Please select a skill:");
            
        }

        if (!NoJoinerTurn)
        {
            await joiner.SendMessageAsync("Unfortunately, you cannot use your skill this turn. Please wait for your opponent to finish their turn.");
        } else
        {
            for (int i = 0; i < JoinerBattle.Skills.Count; i++)
            {
                var skill = JoinerBattle.Skills.ElementAt(i);
                await joiner.SendMessageAsync($"{i + 1}. {skill.Name} - Power: {skill.BasePower} - PP: {skill.PowerPoints}/{skill.MaxPowerPoints}");
            }
            Joinerskill = await joiner.GetInputAsync($"Please select a skill:");
        }

        // Decide who goes first
        if (CreatorBattle.Priority > JoinerBattle.Priority) {

        } else if (CreatorBattle.Priority < JoinerBattle.Priority) {

        } else {
            if (CreatorBattle.Speed > JoinerBattle.Speed) {

            } else if (CreatorBattle.Speed < JoinerBattle.Speed) {

            } else {
                // Randomize
                if (Random.Shared.NextDouble() <= 0.50) {

                } else {

                }
            }
        }




        turn++;
    }

    public void StartBattle()
    {
        // Reset Pokemon Stats for both players
        if (creatorPokemon == null || joinerPokemon == null) return;

        foreach (var pokemon in creatorPokemon) {pokemon.ResetStats();}
        foreach (var pokemon in joinerPokemon) {pokemon.ResetStats();}

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

    public void RestorePokemonStats()
    {

        _backupService.RestorePokemonStats(
            creatorPokemon!, 
            CreatorBattle!, 
            joinerPokemon!, 
            JoinerBattle!, 
            creatorFainted!, 
            joinerFainted!);
        
        _backupService.ClearStatusConditions(
            creatorPokemon!,
            CreatorBattle!,
            joinerPokemon!,
            JoinerBattle!,
            creatorFainted!,
            joinerFainted!);
    }
}
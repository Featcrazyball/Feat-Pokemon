using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.OleDb;
using Arena;
using Database;
using Models;
using Server;

// {Requirements} is required for evolution

namespace PokemonPocket
{
    public class PokemonMaster 
    {
        [Key]
        public string? Id {get;set;}
        public string? Name {get;set;}
        public string? Nickname {get;set;}
        public string? Type {get;set;}
        public int Level {get;set;}
        public int Experience {get;set;}
        public virtual string? Requirements {get;set;}
        public bool Evolvable {get;set;} = false;
        public virtual string? EvolvesTo {get;set;}

        // Feat's Features
        [NotMapped] public float Health {get;set;}
        [NotMapped] public float Attack {get;set;}
        [NotMapped] public float SpecialAttack {get;set;}
        [NotMapped] public float Defense { get;set; } 
        [NotMapped] public float SpecialDefense {get;set;}
        [NotMapped] public float Speed {get;set;}

        public float MaxHealth {get;set;}
        public float MaxAttack {get;set;}
        public float MaxSpecialAttack {get;set;}
        public float MaxDefense { get;set; }
        public float MaxSpecialDefense {get;set;}
        public float MaxSpeed {get;set;}

        [NotMapped] public int AttackStage {get;set;} = 0;
        [NotMapped] public int SpecialAttackStage {get;set;} = 0;
        [NotMapped] public int DefenseStage {get;set;} = 0;
        [NotMapped] public int SpecialDefenseStage {get;set;} = 0;
        [NotMapped] public int SpeedStage {get;set;} = 0;
        [NotMapped] public int AccuracyStage {get;set;} = 0;
        [NotMapped] public int EvasionStage {get;set;} = 0;

        public float CritDmg {get;set;} = 1.5f; // Crit Damage

        private float _critRate {get;set;} // Crit Rate
        public float CritRate
        {
            get => _critRate == 0 ? MaxSpeed / 512 : _critRate;
            set => _critRate = value; 
        }

        // Extra Info
        public int StatPoints {get;set;}
        public int? StatsEarned {get;set;}

        // IV's
        public int HpIV {get;set;}
        public int AttackIV {get;set;}
        public int SpecialAttackIV {get;set;}
        public int DefenseIV {get;set;}
        public int SpecialDefenseIV {get;set;}
        public int SpeedIV {get;set;}

        // Skills
        public virtual string? SkillPool { get; set; }
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

        // Owner
        public string? OwnerId {get;set;}
        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }

        // For Arena
        public bool Selected {get;set;}
        public bool Starter {get;set;} = false;

        // Arena 
        [NotMapped] public Skill? Firstmove { get; set; }
        [NotMapped] public Skill? Lastmove { get; set; }
        [NotMapped] public int Priority { get; set; } = 0;
        [NotMapped] public int PayDay { get; set; } = 0;

        [NotMapped] public bool RazorWindActive { get; set; } = false;

        [NotMapped] public bool Disable { get; set; } = false;
        [NotMapped] public int DisableTurns { get; set; } = 0;
        [NotMapped] public string DisabledSkill { get; set; } = string.Empty;

        [NotMapped] public float BideDamage { get; set; } = 0; 
        [NotMapped] public int BideTurns { get; set; } = 0;
        [NotMapped] public bool BideActive { get; set; } = false;

        [NotMapped] public float BindDamage { get; set; } = 0;
        [NotMapped] public int BindTurns { get; set; } = 0;
        [NotMapped] public bool BindActive { get; set; } = false;

        [NotMapped] public bool Flinch { get; set; } = false;

        [NotMapped] public bool Paralyzed { get; set; } = false;
        [NotMapped] public bool ParalyzeSpeed { get; set; } = false;

        [NotMapped] public bool Burning { get; set; } = false;
        [NotMapped] public bool BurningAttack { get; set; } = false;
        [NotMapped] public float BurnDamage { get; set; } = 0;
        
        [NotMapped] public bool Freezing { get; set; } = false;
        [NotMapped] public bool Poisoned { get; set; } = false;

        [NotMapped] public bool RageActive { get; set; } = false;

        [NotMapped] public bool InAir { get; set; } = false;
        [NotMapped] public bool Levitate { get; set; } = false;
        [NotMapped] public bool Flying { get; set; } = false;

        [NotMapped] public bool Substitude { get; set; } = false;
        [NotMapped] public float SubstituteHealth { get; set; } = 0;

        [NotMapped] public bool Confused { get; set; } = false;
        [NotMapped] public int ConfusionTurns { get; set; } = 0;

        [NotMapped] public bool Dig { get; set; } = false;
        [NotMapped] public float DigDamage { get; set; } = 0;
        [NotMapped] public bool Underground { get; set; } = false;

        [NotMapped] public bool Rest { get; set; } = false;
        [NotMapped] public bool Sleeping { get; set; } = false;
        [NotMapped] public int SleepTurns { get; set; } = 0;

        [NotMapped] public bool HyperBeamRecharge { get; set; } = false;

        [NotMapped] public bool LightScreen { get; set; } = false;
        [NotMapped] public int LightScreenTurns { get; set; } = 0;
        [NotMapped] public bool Reflect { get; set; } = false;
        [NotMapped] public int ReflectTurns { get; set; } = 0;

        [NotMapped] public bool LeechSeed { get; set; } = false;
        [NotMapped] public int LeechSeedTurns { get; set; } = 0;

        [NotMapped] public bool Mist { get; set; } = false;
        [NotMapped] public int MistTurns { get; set; } = 0;

        [NotMapped] public bool Mimic { get; set; } = false;
        [NotMapped] public Skill? MimicSkill { get; set; }

        [NotMapped] public bool PetalDance { get; set; } = false;
        [NotMapped] public int PetalDanceTurns { get; set; } = 0; 
        [NotMapped] public bool Thrashing { get; set; } = false;
        [NotMapped] public int ThrashTurns { get; set; } = 0;

        [NotMapped] public bool ChargingSkull { get; set; } = false;
        [NotMapped] public bool ChargingSky { get; set; } = false;
        [NotMapped] public bool ChargingSolar { get; set; } = false;

        [NotMapped] public bool BadlyPoisoned { get; set; } = false;
        [NotMapped] public int BadlyPoisonedTurns { get; set; } = 0;

        [NotMapped] public bool Transform { get; set; } = false;

        // For Assignment
        public float SkillDamage { get;set; }
        public string? Skill {get;set;}

        public PokemonMaster() { } //For EF Core
        public PokemonMaster(string Name, string Type, float MaxHealth, float MaxAttack, float MaxDefense, float MaxSpecialAttack, float MaxSpecialDefense, float MaxSpeed, string OwnerId, int SkillDamage, string Skill) {
            Id = Guid.NewGuid().ToString("N")[..15]; 
            HpIV = Random.Shared.Next(1, 31);
            AttackIV = Random.Shared.Next(1, 31);
            SpecialAttackIV = Random.Shared.Next(1, 31);
            DefenseIV = Random.Shared.Next(1, 31);
            SpecialDefenseIV = Random.Shared.Next(1, 31);
            SpeedIV = Random.Shared.Next(1, 31);
            StatPoints = Random.Shared.Next(1, 10);
            Level = 1;
            Experience = 0;
            StatsEarned = 0;
            this.Name = Name;
            this.Type = Type;
            this.MaxHealth = MaxHealth;
            this.MaxAttack = MaxAttack;
            this.MaxSpecialAttack = MaxSpecialAttack;
            this.MaxDefense = MaxDefense;
            this.MaxSpecialDefense = MaxSpecialDefense;
            this.MaxSpeed = MaxSpeed;
            this.OwnerId = OwnerId;
            this.Skill = Skill;
            this.SkillDamage = SkillDamage;
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;

            _critRate = 0;
        }

        public PokemonMaster(PokemonMaster poke)
        {
            Id = poke.Id;
            Name = poke.Name;
            Nickname = poke.Nickname;
            Type = poke.Type;
            Level = poke.Level;
            Experience = 0; // Assignment
            Requirements = poke.Requirements;
            Evolvable = poke.Evolvable;

            // Feat's Features
            Health = 100; //Assignment
            Attack = poke.Attack;
            SpecialAttack = poke.SpecialAttack;
            Defense = poke.Defense;
            SpecialDefense = poke.SpecialDefense;
            Speed = poke.Speed;

            MaxHealth = poke.MaxHealth;
            MaxAttack = poke.MaxAttack;
            MaxSpecialAttack = poke.MaxSpecialAttack;
            MaxDefense = poke.MaxDefense;
            MaxSpecialDefense = poke.MaxSpecialDefense;
            MaxSpeed = poke.MaxSpeed;

            AttackStage = 0;
            SpecialAttackStage = 0;
            DefenseStage = 0;
            SpecialDefenseStage = 0;
            SpeedStage = 0;

            _critRate = 0;
            CritDmg = poke.CritDmg;
        }

        // For Assignment
        public virtual float calculateDamage(float SkillDamage) {
            return SkillDamage;
        }

        public void ResetStats()
        {
            // Basic Stats
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;

            AttackStage = 0;
            SpecialAttackStage = 0;
            DefenseStage = 0;
            SpecialDefenseStage = 0;
            SpeedStage = 0;
            AccuracyStage = 0;
            EvasionStage = 0;

            CritRate = MaxSpeed/512; // Crit Rate
            CritDmg = 1.5f; // Crit Damage

            // Arena
            Firstmove =null;
            Lastmove = null;
            Priority  = 0;
            PayDay = 0;
            RazorWindActive = false;
            Disable = false;
            DisableTurns = 0;
            DisabledSkill = string.Empty;
            BideDamage = 0; 
            BideTurns = 0;
            BideActive = false;
            BindDamage = 0;
            BindTurns = 0;
            BindActive = false;
            Flinch = false;
            Paralyzed = false;
            ParalyzeSpeed = false;
            Burning = false;
            BurningAttack = false;
            BurnDamage = 0;
            Freezing = false;
            Poisoned = false;
            RageActive = false;
            InAir = false;
            Levitate = false;
            Flying = false;
            Substitude = false;
            SubstituteHealth = 0;
            Confused = false;
            ConfusionTurns = 0;
            Dig = false;
            DigDamage = 0;
            Underground = false;
            Rest = false;
            Sleeping = false;
            SleepTurns = 0;
            HyperBeamRecharge = false;
            LightScreen = false;
            LightScreenTurns = 0;
            Reflect = false;
            ReflectTurns = 0;
            LeechSeed = false;
            LeechSeedTurns = 0;
            Mist = false;
            MistTurns = 0;
            Mimic = false;
            PetalDance = false;
            PetalDanceTurns = 0; 
            Thrashing = false;
            ThrashTurns = 0;
            ChargingSkull = false;
            ChargingSky = false;
            ChargingSolar = false;
            BadlyPoisoned = false;
            BadlyPoisonedTurns = 0;
            Transform = false;
        }

        // For Re-calculation DO NOT TOUCH
        public void EvolveLevelUp(int times)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 101) {
                    // Stat Upgrades
                    Level += 1;
                    MaxHealth += (MaxHealth + HpIV) / 50 + Level + 10;
                    MaxAttack += (MaxAttack + AttackIV) / 8 + 1; 
                    MaxSpecialAttack += (MaxSpecialAttack + SpecialAttackIV) / 8 + 1; 
                    MaxDefense += (MaxDefense + DefenseIV) / 8 + 1; 
                    MaxSpecialDefense += (MaxSpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    MaxSpeed += (MaxSpeed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                }
            }
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;
            CritRate = MaxSpeed/512;
        }

        // For Leveling Up
        public async Task LevelUp(int times, ClientSession session)
        {
            for (int i = 0; i < times; i++)
            {
                if (Experience > Level * 1000 && Level < 101) {
                    // Stat Upgrades
                    Level += 1;
                    Experience -= 1000;
                    MaxHealth += (MaxHealth + HpIV) / 50 + Level + 10;
                    MaxAttack += (MaxAttack + AttackIV) / 8 + 1; 
                    MaxSpecialAttack += (MaxSpecialAttack + SpecialAttackIV) / 8 + 1; 
                    MaxDefense += (MaxDefense + DefenseIV) / 8 + 1; 
                    MaxSpecialDefense += (MaxSpecialDefense + SpecialDefenseIV) / 8 + 1; 
                    MaxSpeed += (MaxSpeed + SpeedIV) / 8 + 1;

                    // Make sure there is a max of 250 stat points earned
                    for (int j = 0; j < 3; j++)
                    {
                        if (StatsEarned < 251) {StatPoints += 1; StatsEarned += 1;}
                    }
                    await session.SendMessageAsync($"Your {Name} has leveled up to level {Level}!\nYou have {StatPoints} Stat Points left.");

                    // Max Level Check
                    if (Level >= 100) { await session.SendMessageAsync($"Your {Name} has reached a max level of 100!"); break;}
                } 
            }
            Health = MaxHealth;
            Attack = MaxAttack;
            SpecialAttack = MaxSpecialAttack;
            Defense = MaxDefense;
            SpecialDefense = MaxSpecialDefense;
            Speed = MaxSpeed;
            CritRate = MaxSpeed/512;
        }

        // Check if can evolve
        public string CheckEvolve()
        {
            if (Name == "Eevee")
            {
                using var context = new DatabaseContext();

                var item1 = context.Items.Where(i => i.Name == "Water Stone" && i.OwnerId == OwnerId);
                var item2 = context.Items.Where(i => i.Name == "Fire Stone" && i.OwnerId == OwnerId);
                var item3 = context.Items.Where(i => i.Name == "Leaf Stone" && i.OwnerId == OwnerId);

                if (item1.Any() || item2.Any() || item3.Any())
                {
                    return "true item | 1 WFT Stone";
                } else {
                    return "false item";
                }
            }
            if (Requirements!.Contains("Level"))
            {
                int req = int.Parse(Requirements.Split(' ')[1]);
                return Level >= req ? "true level" : "false level";

            } else if (Requirements.Contains("Stone"))
            {
                var req = Requirements.Split(' ').Skip(1);
                string count = Requirements.Split(' ')[0];
                string stringReq = string.Empty;

                foreach (var ch in req)
                {
                    stringReq += ch.Trim() + " ";
                }
                
                stringReq = stringReq.Trim();
                using var context = new DatabaseContext();
                var item = context.Items.FirstOrDefault(i => i.Name == stringReq);

                return item != null ? $"true item|{count} {stringReq}" : "false item";
            
            } else if (Requirements.Contains("Trade"))
            {
                return "true trade";
            } else if (Requirements.Contains("Unevolvable"))
            {
                return "false unevolvable";
            }
            return "false null";
        }

        // Stat Points Management
        public async Task AddStatPoints(int points, ClientSession session)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatsEarned < 251) { StatPoints += 1; StatsEarned -= 1; }
                else { break; }
            }
            await session.SendMessageAsync($"Stat Points have been added to your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        public async Task RemoveStatPoints(int points, ClientSession session)
        {
            for (int i = 0; i < points; i++)
            {
                if (StatPoints > 0) { StatPoints -= 1;}
                else { break; }
            }
            await session.SendMessageAsync($"Stat Points have been removed from your Pokemon!\nYou have {StatPoints} Stat Points left.");
        }

        public async Task AssignStatPoints(int points, string stat, ClientSession session)
        {
            if (points > StatPoints)
            {
                await session.SendMessageAsync($"You do not have enough Stat Points to assign {points} points to {stat}.");
                return;
            }

            for (int i = 0; i < points; i++)
            {
                if (StatPoints > 0) { StatPoints -= 1;}
                else { break; }
                switch(stat.ToLower())
                {
                    case "health":
                        MaxHealth += 1;
                        break;
                    case "attack":
                        MaxAttack += 1;
                        break;
                    case "specialattack":
                        MaxSpecialAttack += 1;
                        break;
                    case "defense":
                        MaxDefense += 1;
                        break;
                    case "specialdefense":
                        MaxSpecialDefense += 1;
                        break;
                    case "speed":
                        MaxSpeed += 1;
                        break;
                    default:
                        await session.SendMessageAsync($"Invalid stat: {stat}. Please choose from Health, Attack, Special Attack, Defense, Special Defense, or Speed.");
                        break;
                }
            }

            if (stat.ToLower() == "health")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxHealth}.");
            else if (stat.ToLower() == "attack")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxAttack}.");
            else if (stat.ToLower() == "special attack")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxSpecialAttack}.");
            else if (stat.ToLower() == "defense")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxDefense}.");
            else if (stat.ToLower() == "special defense")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxSpecialDefense}.");
            else if (stat.ToLower() == "speed")
                await session.SendMessageAsync($"Your {Name} has gained {points} {stat} points!\nYour {stat} is now {MaxSpeed}.");
        }

        // Base Form Evolve (dont bother)
        public virtual async Task Evolve(ClientSession session)
        {
            await session.SendMessageAsync($"{Nickname} is unable evolve.");
        }

        // Skill Management (Complete)
        public async Task LearnSkill(string skillName, ClientSession session)
        {
            using var context = new DatabaseContext();
            
            // Check if already knows 4 skills
            var currentSkillCount = context.Skills.Count(s => s.PokemonId == Id);
            if (currentSkillCount >= 4)
            {
                await session.SendMessageAsync($"{Nickname} already knows 4 skills. It must forget one first.");
                return;
            }
            
            // Find a skill template with this name
            var skillTemplate = context.Skills.FirstOrDefault(s => s.Name == skillName && s.PokemonId == null);
            if (skillTemplate == null)
            {
                await session.SendMessageAsync($"{skillName} does not exist.");
                return;
            }
            
            // Create skill
            Skill newSkill = ArenaTempSkillGain(skillName.ToLower())!;

            if (newSkill == null)
            {
                await session.SendMessageAsync($"{skillName} is not a valid skill.");
                return;
            }
            
            // Add to database
            context.Skills.Add(newSkill);
            context.SaveChanges();
            
            await session.SendMessageAsync($"{Nickname} has learned {skillName}!");
        }

        public async Task ForgetSkill(string skillName, ClientSession session)
        {
            using var context = new DatabaseContext();
            
            // Find the skill
            var skill = context.Skills.FirstOrDefault(s => s.Name == skillName && s.PokemonId == Id);
            if (skill == null)
            {
                await session.SendMessageAsync($"{Name} does not know {skillName}.");
                return;
            }
            
            // Remove from database
            context.Skills.Remove(skill);
            context.SaveChanges();
            
            Console.WriteLine($"{Name} has forgotten {skillName}!");
        }

        public void ForgetTillFive()
        {
            if (this.Skills.Count > 5)
            {
                foreach(var skill in this.Skills)
                {
                    // Randomly delete 1 skill
                    if (this.Skills.Count > 5)
                    {
                        Random random = new Random();
                        int index = random.Next(this.Skills.Count);
                        var skillToRemove = this.Skills.ElementAt(index);
                        this.Skills.Remove(skillToRemove);
                        using (var context = new DatabaseContext())
                        {
                            context.Skills.Remove(skillToRemove);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }

        // Skill Pool Management (Complete)
        public List<Skill> LearnSkillFromSkillPool()
        {
            List<Skill> newSkills = new List<Skill>();
            if (string.IsNullOrEmpty(SkillPool)) return newSkills;
            
            var skills = SkillPool.Split(',').Select(s => s.Trim()).ToList();
            Random random = new Random();
            
            // Choose random skills (up to 4)
            int numSkills = Random.Shared.Next(1, 5);
            
            if (SkillPool.Length < numSkills)
            {
                numSkills = SkillPool.Length;
            }

            for (int i = 0; i < numSkills; i++)
            {
                while (true)
                {
                    int index = random.Next(skills.Count);
                    var skillName = skills[index];
                    skills.RemoveAt(index);
                    
                    // Create skill but don't save directly
                    var skill = ArenaTempSkillGain(skillName.ToLower());
                    
                    if (skill == null)
                    {
                        Console.WriteLine($"Skill {skillName} not found.");
                        continue;
                    }
                    newSkills.Add(skill);
                    break;
                }
            }
            // Instead of saving here with context.SaveChanges(), return the skills
            return newSkills;
        }

        public void SetStarter(PokemonMaster poke) 
        {
            using var context = new DatabaseContext();
            foreach (var pokemon in poke.Owner?.Pokemon ?? new List<PokemonMaster>())
            {
                if (pokemon.Starter) { pokemon.Starter = false; }
            }
            poke.Starter = true;
            context.SaveChanges();
        }
    
        public Skill? ArenaTempSkillGain(string skillName)
        {
            return skillName switch
            {
                "absorb" => new Absorb(Id!), "acid" => new Acid(Id!), "acid armor" => new AcidArmor(Id!),
                "agility" => new Agility(Id!), "amnesia" => new Amnesia(Id!), "aurora beam" => new AuroraBeam(Id!),
                "barrage" => new Barrage(Id!), "barrier" => new Models.Barrier(Id!), "bide" => new Bide(Id!),
                "bind" => new Bind(Id!), "bite" => new Bite(Id!), "blizzard" => new Blizzard(Id!),
                "body slam" => new BodySlam(Id!), "bone club" => new BoneClub(Id!), "bonemerang" => new Bonemerang(Id!),
                "bubble" => new Bubble(Id!), "bubble beam" => new BubbleBeam(Id!), "clamp" => new Clamp(Id!),
                "comet punch" => new CometPunch(Id!), "confuse ray" => new ConfuseRay(Id!), "confusion" => new Confusion(Id!),
                "constrict" => new Constrict(Id!), "conversion" => new Conversion(Id!), "counter" => new Counter(Id!),
                "crabhammer" => new Crabhammer(Id!), "cut" => new Cut(Id!), "defense curl" => new DefenseCurl(Id!),
                "dig" => new Dig(Id!), "disable" => new Disable(Id!), "dizzy punch" => new DizzyPunch(Id!),
                "double-edge" => new DoubleEdge(Id!), "double kick" => new DoubleKick(Id!), "double slap" => new DoubleSlap(Id!),
                "double team" => new DoubleTeam(Id!), "dragon rage" => new DragonRage(Id!), "dream eater" => new DreamEater(Id!),
                "drill peck" => new DrillPeck(Id!), "earthquake" => new Earthquake(Id!), "egg bomb" => new EggBomb(Id!),
                "ember" => new Ember(Id!), "explosion" => new Explosion(Id!), "fire blast" => new FireBlast(Id!),
                "fire punch" => new FirePunch(Id!), "fire spin" => new FireSpin(Id!), "fissure" => new Fissure(Id!),
                "flamethrower" => new Flamethrower(Id!), "flash" => new Flash(Id!), "fly" => new Fly(Id!),
                "focus energy" => new FocusEnergy(Id!), "fury attack" => new FuryAttack(Id!), "fury swipes" => new FurySwipes(Id!),
                "glare" => new Glare(Id!), "growl" => new Growl(Id!), "growth" => new Growth(Id!),
                "guillotine" => new Guillotine(Id!), "gust" => new Gust(Id!), "harden" => new Harden(Id!),
                "haze" => new Haze(Id!), "headbutt" => new Headbutt(Id!), "high jump kick" => new HighJumpKick(Id!),
                "horn attack" => new HornAttack(Id!), "horn drill" => new HornDrill(Id!), "hydro pump" => new HydroPump(Id!),
                "hyper beam" => new HyperBeam(Id!), "hyper fang" => new HyperFang(Id!), "hypnosis" => new Hypnosis(Id!),
                "ice beam" => new IceBeam(Id!), "ice punch" => new IcePunch(Id!), "jump kick" => new JumpKick(Id!),
                "karate chop" => new KarateChop(Id!), "kinesis" => new Kinesis(Id!), "leech life" => new LeechLife(Id!),
                "leech seed" => new LeechSeed(Id!), "leer" => new Leer(Id!), "lick" => new Lick(Id!),
                "light screen" => new LightScreen(Id!), "lovely kiss" => new LovelyKiss(Id!), "low kick" => new LowKick(Id!),
                "meditate" => new Meditate(Id!), "mega drain" => new MegaDrain(Id!), "mega kick" => new MegaKick(Id!),
                "mega punch" => new MegaPunch(Id!), "metronome" => new Metronome(Id!), "mimic" => new Mimic(Id!),
                "minimize" => new Minimize(Id!), "mirror move" => new MirrorMove(Id!), "mist" => new Mist(Id!),
                "night shade" => new NightShade(Id!), "pay day" => new PayDay(Id!), "peck" => new Peck(Id!),
                "petal dance" => new PetalDance(Id!), "pin missile" => new PinMissile(Id!), "poison gas" => new PoisonGas(Id!),
                "poison powder" => new PoisonPowder(Id!), "poison sting" => new PoisonSting(Id!), "pound" => new Pound(Id!),
                "psybeam" => new Psybeam(Id!), "psychic" => new Psychic(Id!), "psywave" => new Psywave(Id!),
                "quick attack" => new QuickAttack(Id!), "rage" => new Rage(Id!), "razor leaf" => new RazorLeaf(Id!),
                "razor wind" => new RazorWind(Id!), "recover" => new Recover(Id!), "reflect" => new Reflect(Id!),
                "rest" => new Rest(Id!), "roar" => new Roar(Id!), "rock slide" => new RockSlide(Id!),
                "rock throw" => new RockThrow(Id!), "rolling kick" => new RollingKick(Id!), "sand attack" => new SandAttack(Id!),
                "scratch" => new Scratch(Id!), "screech" => new Screech(Id!), "seismic toss" => new SeismicToss(Id!),
                "self-destruct" => new SelfDestruct(Id!), "sharpen" => new Sharpen(Id!), "sing" => new Sing(Id!),
                "skull bash" => new SkullBash(Id!), "sky attack" => new SkyAttack(Id!), "slam" => new Slam(Id!),
                "slash" => new Slash(Id!), "sleep powder" => new SleepPowder(Id!), "sludge" => new Sludge(Id!),
                "smog" => new Smog(Id!), "smokescreen" => new Smokescreen(Id!), "soft-boiled" => new SoftBoiled(Id!),
                "solar beam" => new SolarBeam(Id!), "sonic boom" => new SonicBoom(Id!), "spike cannon" => new SpikeCannon(Id!),
                "splash" => new Splash(Id!), "spore" => new Spore(Id!), "stomp" => new Stomp(Id!),
                "strength" => new Strength(Id!), "string shot" => new StringShot(Id!), "struggle" => new Struggle(Id!),
                "stun spore" => new StunSpore(Id!), "submission" => new Submission(Id!), "substitute" => new Substitute(Id!),
                "super fang" => new SuperFang(Id!), "supersonic" => new Supersonic(Id!), "surf" => new Surf(Id!),
                "swift" => new Swift(Id!), "swords dance" => new SwordsDance(Id!), "tackle" => new Tackle(Id!),
                "tail whip" => new TailWhip(Id!), "take down" => new TakeDown(Id!), "teleport" => new Teleport(Id!),
                "thrash" => new Thrash(Id!), "thunder" => new Thunder(Id!), "thunderbolt" => new Thunderbolt(Id!),
                "thunder punch" => new ThunderPunch(Id!), "thunder shock" => new ThunderShock(Id!), "thunder wave" => new ThunderWave(Id!),
                "toxic" => new Toxic(Id!), "transform" => new Transform(Id!), "tri attack" => new TriAttack(Id!),
                "twineedle" => new Twineedle(Id!), "vine whip" => new VineWhip(Id!), "vise grip" => new ViseGrip(Id!),
                "waterfall" => new Waterfall(Id!), "water gun" => new WaterGun(Id!), "whirlwind" => new Whirlwind(Id!),
                "wing attack" => new WingAttack(Id!), "withdraw" => new Withdraw(Id!), "wrap" => new Wrap(Id!),
                _ => null,
            };
        }

    }
}
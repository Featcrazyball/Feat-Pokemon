# Feat-Pokémon

A multiplayer, turn-based Pokémon battle game that runs in the terminal, built from scratch in **C# / .NET 9**. Players connect to a central TCP server, build a team, and fight other players in real time in an online arena.

All 151 original Pokémon and 165 of their moves are implemented, each as its own class, with type matchups, status conditions, stat stages, critical hits and evolution.

> Fan project made for learning. Pokémon and all related names belong to Nintendo, Game Freak and The Pokémon Company. This project is not affiliated with them and is not sold or monetised in any way.

---

## Highlights

- **Client–server networking from first principles.** Raw `System.Net.Sockets` TCP: one async session per player, up to 30 at once, with a background loop that detects and cleans up dropped connections.
- **Real-time PvP arena.** Players create or join battle rooms. Both players pick an action each turn, and the server works out turn order from speed and move priority, then resolves damage, status effects, fainting and forced switches.
- **A faithful battle engine.** An 18-type effectiveness chart, accuracy and evasion stages, critical hits, physical vs. special damage, and two-turn moves (Dig, Fly, Solar Beam). Status conditions are implemented too: burn, poison, bad poison, sleep, paralysis, freeze and confusion.
- **Object-oriented game model.** Every Pokémon (`Models/Pokemons/`) and every move (`Models/Skills/`) is a subclass of a shared base class, so a new one can be added without touching the battle engine.
- **Persistent world.** Accounts, Pokémon, learned moves and items are stored in SQLite through Entity Framework Core. EF Core's table-per-hierarchy mapping is set up by reflection, so every Pokémon and move subclass is registered automatically.
- **A full game loop around the battles.** A shop (Pokéballs, evolution stones, XP bottles), a coin economy with battle rewards, level-up / stone / trade evolution, lineup and starter selection, and account settings.

---

## Architecture

![Feat-Pokémon architecture diagram](docs/architecture.jpg)

The system has four layers:

| Layer | What it does | Key files |
|---|---|---|
| **Client and access** | A thin console client sends what the player types to the server and prints what comes back. The server accepts connections and gives each player a `ClientSession`. | `client/Program.cs`, `server/Client/Network.cs`, `Session.cs`, `Loop.cs` |
| **Player features** | Menu-driven features the network layer sends commands to: Pokémon, Arena, Battle, Evolution, Shop, Lineup, Settings. | `server/Client/Functions.cs/*` |
| **Game domain** | Pokémon, moves, players, arena state, and the damage and type calculations. | `server/Models/*`, `server/Calculators/SkillHelper.cs`, `Type.cs` |
| **Persistence** | The EF Core `DatabaseContext` on top of a SQLite file. | `server/Database/Database.cs`, `server/Migrations/` |

The client holds no game state. All logic and data live on the server, so players can't change their own coins or Pokémon locally.

---

## Gameplay

1. **Register or log in.** New trainers start with 100 coins and five random Pokémon.
2. **Trainer menu**
   - **Pokémon**: view your collection, stats and moves, and evolve by level, stone or trade.
   - **Shop**: buy Pokéballs (a random Pokémon), elemental stones and XP bottles; view your inventory.
   - **Lineup**: choose the 6 Pokémon you'll battle with and set 1 as your starter.
   - **Arena**: create, join or refresh battle rooms and fight another player live.
   - **Settings**: change your username, email or password.
3. **Battle.** Each turn you either attack with one of your Pokémon's moves or switch Pokémon. The first trainer to knock out the whole opposing team wins. Both players get coins, plus a bonus from Pay Day.

There is also an admin (“God”) account with tools to create Pokémon and moderate users (for example, banning them).

---

## Tech stack

- **Language / runtime:** C# on .NET 9
- **Networking:** `System.Net.Sockets` (TCP, async/await)
- **Data:** Entity Framework Core 9 + SQLite
- **Interface:** console UI with box-drawing characters

---

## Project structure

```
Feat-Pokemon/
├── client/
│   └── Program.cs              # Console client: connect, send input, print output
└── server/
    ├── Program.cs              # Entry point: DB init, socket setup, accept loop
    ├── Client/
    │   ├── Network.cs          # Socket setup, login/registration, session registry
    │   ├── Session.cs          # Per-player send/receive wrapper
    │   ├── Loop.cs             # Main trainer menu
    │   └── Functions.cs/       # Feature modules (Arena, Pokémon, Shop, Lineup, Settings…)
    ├── Models/
    │   ├── PokeMaster.cs       # Base Pokémon class (stats, stages, status flags)
    │   ├── Pokemons/           # 151 Pokémon subclasses
    │   ├── Skill.cs            # Base move class
    │   ├── Skills/             # 165 move subclasses
    │   ├── Arena.cs            # Battle state machine
    │   ├── User.cs, Item.cs, List.cs
    ├── Calculators/
    │   ├── SkillHelper.cs      # Damage, accuracy, crits, stat stages, two-turn moves
    │   └── Type.cs             # Type-effectiveness chart
    ├── Database/Database.cs    # EF Core DbContext
    └── Migrations/
```

---

## Running it locally

**Requirement:** the [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).

```bash
git clone https://github.com/Featcrazyball/Feat-Pokemon.git
cd Feat-Pokemon

# 1. Start the server (creates database.db if it doesn't exist; listens on port 8000)
cd server
dotnet run
```

The server prints the address it's listening on. Set the client to that address by changing the `IPAddress.Parse(...)` line near the top of `client/Program.cs`. Then, in a second terminal:

```bash
cd client
dotnet run
```

Open more client terminals, on the same machine or others on the network, to have several players battle each other. Type `exit` to quit the client.

---

## What I learned

- Designing a **stateful, concurrent server**: one session per player, sharing arena rooms safely between two sessions, and handling disconnects partway through a battle.
- Using **inheritance and polymorphism** at scale. Over 300 game entities share one engine, and each move's special behaviour stays inside its own class.
- Using an **ORM** for real: mapping a deep class hierarchy to one table with discriminators, and managing schema changes with migrations.
- Turning a complicated real-world rule set (Gen 1 battle mechanics) into code that is deterministic where it should be and random where it should be.

## Limitations and next steps

Things I'd improve next:

- **Configuration:** read the server address and port from a config file or command-line arguments instead of hard-coding them.
- **Chat:** finish the trainer chat, which is in the code but currently turned off.
- **Testing:** add unit tests for the damage formula and type chart.
- **Interface:** build a graphical or web client on top of the same server protocol.

---

## Author

**Yang Xuanming, Xavier** ([@Featcrazyball](https://github.com/Featcrazyball))


**Yang Xuanming, Xavier** ([@Featcrazyball](https://github.com/Featcrazyball))

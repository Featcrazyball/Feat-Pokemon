using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class MoveNicknameToBaseClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Nickname = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<float>(type: "REAL", nullable: false),
                    MaxHealth = table.Column<float>(type: "REAL", nullable: false),
                    Attack = table.Column<float>(type: "REAL", nullable: false),
                    SpecialAttack = table.Column<float>(type: "REAL", nullable: false),
                    Defense = table.Column<float>(type: "REAL", nullable: false),
                    SpecialDefense = table.Column<float>(type: "REAL", nullable: false),
                    Speed = table.Column<float>(type: "REAL", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    StatPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    StatsEarned = table.Column<int>(type: "INTEGER", nullable: true),
                    HpIV = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackIV = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialAttackIV = table.Column<int>(type: "INTEGER", nullable: false),
                    DefenseIV = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialDefenseIV = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedIV = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillPool = table.Column<string>(type: "TEXT", nullable: true),
                    SkillDamage = table.Column<float>(type: "REAL", nullable: false),
                    Skill = table.Column<string>(type: "TEXT", nullable: true),
                    PokemonType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false),
                    Coins = table.Column<int>(type: "INTEGER", nullable: false),
                    FeatVersion = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    BasePower = table.Column<int>(type: "INTEGER", nullable: false),
                    Accuracy = table.Column<float>(type: "REAL", nullable: false),
                    LevelRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Cooldown = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    InUse = table.Column<bool>(type: "INTEGER", nullable: false),
                    PokemonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_PokemonMaster_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "PokemonMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_PokemonId",
                table: "Skills",
                column: "PokemonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PokemonMaster");
        }
    }
}

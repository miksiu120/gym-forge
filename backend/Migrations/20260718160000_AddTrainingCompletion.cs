using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WorkPlanner.Entities;

#nullable disable

namespace WorkPlanner.Migrations
{
    [DbContext(typeof(WorkPlannerDbContext))]
    [Migration("20260718160000_AddTrainingCompletion")]
    public partial class AddTrainingCompletion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "TrainingUnits",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "TrainingUnits",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseSetResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: true),
                    Repetitions = table.Column<int>(type: "integer", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: true),
                    Rpe = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSetResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseSetResults_TimeExercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "TimeExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSetResults_ExerciseId",
                table: "ExerciseSetResults",
                column: "ExerciseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ExerciseSetResults");
            migrationBuilder.DropColumn(name: "CompletedAt", table: "TrainingUnits");
            migrationBuilder.DropColumn(name: "Notes", table: "TrainingUnits");
        }
    }
}

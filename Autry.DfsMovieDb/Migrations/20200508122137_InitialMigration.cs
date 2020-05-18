using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Autry.DfsMovieDb.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Genre = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.MovieId);
                });

            migrationBuilder.CreateTable(
                name: "MovieActor",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    ActorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActor", x => new { x.MovieId, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MovieActor_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "ActorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieActor_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "MovieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "ActorId", "Birthdate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 10, new DateTime(1976, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "Doe" },
                    { 11, new DateTime(1964, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keanu", "Reeves" },
                    { 12, new DateTime(1961, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laurence", "Fishburne" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "MovieId", "Genre", "Title", "Year" },
                values: new object[,]
                {
                    { 20, "Sci-Fi", "The Matrix", 1999 },
                    { 21, "Sci-Fi", "The Matrix Reloaded", 2003 },
                    { 22, "Comedy", "Bill & Ted's Excellent Adventure", 1999 }
                });

            migrationBuilder.InsertData(
                table: "MovieActor",
                columns: new[] { "MovieId", "ActorId" },
                values: new object[,]
                {
                    { 20, 11 },
                    { 20, 12 },
                    { 21, 11 },
                    { 21, 12 },
                    { 22, 11 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieActor_ActorId",
                table: "MovieActor",
                column: "ActorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieActor");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}

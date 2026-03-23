using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registreringstjansten.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registreringar",
                columns: table => new
                {
                    RegistreringId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registreringar", x => x.RegistreringId);
                });

            migrationBuilder.CreateTable(
                name: "StatusHistorik",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RegistrationId = table.Column<int>(type: "INTEGER", nullable: false),
                    OldStatus = table.Column<string>(type: "TEXT", nullable: true),
                    NewStatus = table.Column<string>(type: "TEXT", nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RegistreringId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusHistorik", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_StatusHistorik_Registreringar_RegistreringId",
                        column: x => x.RegistreringId,
                        principalTable: "Registreringar",
                        principalColumn: "RegistreringId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusHistorik_RegistreringId",
                table: "StatusHistorik",
                column: "RegistreringId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusHistorik");

            migrationBuilder.DropTable(
                name: "Registreringar");
        }
    }
}

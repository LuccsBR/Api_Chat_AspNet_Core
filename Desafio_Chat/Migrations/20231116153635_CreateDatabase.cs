using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Chat.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Mensagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id_Usuario_EEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Id_Usuario_REmail = table.Column<string>(type: "TEXT", nullable: false),
                    Texto = table.Column<string>(type: "TEXT", nullable: true),
                    horario = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    New = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensagens_Users_Id_Usuario_EEmail",
                        column: x => x.Id_Usuario_EEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensagens_Users_Id_Usuario_REmail",
                        column: x => x.Id_Usuario_REmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_Id_Usuario_EEmail",
                table: "Mensagens",
                column: "Id_Usuario_EEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_Id_Usuario_REmail",
                table: "Mensagens",
                column: "Id_Usuario_REmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensagens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

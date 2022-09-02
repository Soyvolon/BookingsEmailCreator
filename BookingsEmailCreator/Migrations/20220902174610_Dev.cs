using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingsEmailCreator.Migrations
{
    public partial class Dev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    MsId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Tempalte = table.Column<string>(type: "TEXT", nullable: false),
                    AccountKey = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Key);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Accounts_AccountKey",
                        column: x => x.AccountKey,
                        principalTable: "Accounts",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_AccountKey",
                table: "EmailTemplates",
                column: "AccountKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}

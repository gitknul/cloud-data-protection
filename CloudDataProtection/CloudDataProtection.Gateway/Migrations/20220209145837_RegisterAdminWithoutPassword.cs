using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudDataProtection.Migrations
{
    public partial class RegisterAdminWithoutPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordSetAt",
                table: "User",
                type: "timestamp without time zone",
                nullable: true);
            
            migrationBuilder.Sql("UPDATE \"User\" SET \"CreatedAt\" = NOW()");
            migrationBuilder.Sql("UPDATE \"User\" SET \"PasswordSetAt\" = NOW()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordSetAt",
                table: "User");
        }
    }
}

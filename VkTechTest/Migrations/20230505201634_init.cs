using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VkTechTest.Models.Enums;
using VkTechTest.Services.Implementations;

#nullable disable

namespace VkTechTest.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_state",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_state", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "date", nullable: false),
                    user_group_id = table.Column<long>(type: "bigint", nullable: false),
                    user_state_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_user_group_user_group_id",
                        column: x => x.user_group_id,
                        principalTable: "user_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_user_state_user_state_id",
                        column: x => x.user_state_id,
                        principalTable: "user_state",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_login",
                table: "user",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_user_group_id",
                table: "user",
                column: "user_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_user_state_id",
                table: "user",
                column: "user_state_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_group_code",
                table: "user_group",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_state_code",
                table: "user_state",
                column: "code",
                unique: true);

            int adminGroupNumber = (int)UserGroupType.Admin;
            int userGroupNumber = (int)UserGroupType.User;

            int activeUserStateNumber = (int)UserStateType.Active;
            int blockedUserStateNumber = (int)UserStateType.Blocked;
            
            migrationBuilder.Sql($"INSERT INTO user_group (id, code, description) VALUES ({adminGroupNumber}, {adminGroupNumber}, 'Admin group')");
            migrationBuilder.Sql($"INSERT INTO user_group (id, code, description) VALUES ({userGroupNumber}, {userGroupNumber}, 'User group')");
            
            migrationBuilder.Sql($"INSERT INTO user_state (id, code, description) VALUES ({activeUserStateNumber}, {activeUserStateNumber}, 'Active user state')");
            migrationBuilder.Sql($"INSERT INTO user_state (id, code, description) VALUES ({blockedUserStateNumber}, {blockedUserStateNumber}, 'Blocked user state')");

            var passwordHasher = new SHA256PasswordHasher();
            var hashedPassword = passwordHasher.Hash("admin");
            var adminLogin = "admin";

            migrationBuilder.Sql(
                $"INSERT INTO users (login, password, user_group_id, user_state_id) VALUES ({adminLogin}, {hashedPassword}, {adminGroupNumber}, {activeUserStateNumber})");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "user_group");

            migrationBuilder.DropTable(
                name: "user_state");
        }
    }
}

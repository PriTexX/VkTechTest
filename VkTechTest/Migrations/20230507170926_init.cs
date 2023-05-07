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
                name: "user_groups",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_states",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    user_group_id = table.Column<long>(type: "bigint", nullable: false),
                    user_state_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_user_groups_user_group_id",
                        column: x => x.user_group_id,
                        principalTable: "user_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_users_user_states_user_state_id",
                        column: x => x.user_state_id,
                        principalTable: "user_states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_groups_code",
                table: "user_groups",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_states_code",
                table: "user_states",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_user_group_id",
                table: "users",
                column: "user_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_state_id",
                table: "users",
                column: "user_state_id");
            
            int adminGroupNumber = (int)UserGroupType.Admin;
            int userGroupNumber = (int)UserGroupType.User;

            int activeUserStateNumber = (int)UserStateType.Active;
            int blockedUserStateNumber = (int)UserStateType.Blocked;
            
            migrationBuilder.Sql($"INSERT INTO user_groups (id, code, description) VALUES ({adminGroupNumber}, {adminGroupNumber}, 'Admin group')");
            migrationBuilder.Sql($"INSERT INTO user_groups (id, code, description) VALUES ({userGroupNumber}, {userGroupNumber}, 'User group')");
            
            migrationBuilder.Sql($"INSERT INTO user_states (id, code, description) VALUES ({activeUserStateNumber}, {activeUserStateNumber}, 'Active user state')");
            migrationBuilder.Sql($"INSERT INTO user_states (id, code, description) VALUES ({blockedUserStateNumber}, {blockedUserStateNumber}, 'Blocked user state')");

            var passwordHasher = new SHA256PasswordHasher();
            var hashedPassword = passwordHasher.Hash("admin");
            var adminLogin = "admin";

            migrationBuilder.Sql(
                $"INSERT INTO users (login, password, user_group_id, user_state_id) VALUES ('{adminLogin}', '{hashedPassword}', {adminGroupNumber}, {activeUserStateNumber})");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "user_groups");

            migrationBuilder.DropTable(
                name: "user_states");
        }
    }
}

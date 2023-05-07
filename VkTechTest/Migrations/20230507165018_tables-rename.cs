using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VkTechTest.Migrations
{
    /// <inheritdoc />
    public partial class tablesrename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_user_group_user_group_id",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_state_user_state_id",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_state",
                table: "user_state");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_group",
                table: "user_group");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user_state",
                newName: "user_states");

            migrationBuilder.RenameTable(
                name: "user_group",
                newName: "user_groups");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_user_state_code",
                table: "user_states",
                newName: "IX_user_states_code");

            migrationBuilder.RenameIndex(
                name: "IX_user_group_code",
                table: "user_groups",
                newName: "IX_user_groups_code");

            migrationBuilder.RenameIndex(
                name: "IX_user_user_state_id",
                table: "users",
                newName: "IX_users_user_state_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_user_group_id",
                table: "users",
                newName: "IX_users_user_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_login",
                table: "users",
                newName: "IX_users_login");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_states",
                table: "user_states",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_groups",
                table: "user_groups",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_user_groups_user_group_id",
                table: "users",
                column: "user_group_id",
                principalTable: "user_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_user_states_user_state_id",
                table: "users",
                column: "user_state_id",
                principalTable: "user_states",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_user_groups_user_group_id",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_user_states_user_state_id",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_states",
                table: "user_states");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_groups",
                table: "user_groups");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "user_states",
                newName: "user_state");

            migrationBuilder.RenameTable(
                name: "user_groups",
                newName: "user_group");

            migrationBuilder.RenameIndex(
                name: "IX_users_user_state_id",
                table: "user",
                newName: "IX_user_user_state_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_user_group_id",
                table: "user",
                newName: "IX_user_user_group_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_login",
                table: "user",
                newName: "IX_user_login");

            migrationBuilder.RenameIndex(
                name: "IX_user_states_code",
                table: "user_state",
                newName: "IX_user_state_code");

            migrationBuilder.RenameIndex(
                name: "IX_user_groups_code",
                table: "user_group",
                newName: "IX_user_group_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_state",
                table: "user_state",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_group",
                table: "user_group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_group_user_group_id",
                table: "user",
                column: "user_group_id",
                principalTable: "user_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_state_user_state_id",
                table: "user",
                column: "user_state_id",
                principalTable: "user_state",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

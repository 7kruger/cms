using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseWork.DAL.Migrations
{
    public partial class UpdateComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "Modified");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_CreatorId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Parent",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpvoteCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    UpvotedCommentsId = table.Column<string>(type: "nvarchar(450)", nullable: false, name: "CommentId"),
                    UpvotedUsersId = table.Column<int>(type: "int", nullable: false, name: "UserId")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.UpvotedCommentsId, x.UpvotedUsersId });
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_UpvotedCommentsId",
                        column: x => x.UpvotedCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Users_UpvotedUsersId",
                        column: x => x.UpvotedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 3, 4, 4, 46, 54, 981, DateTimeKind.Local).AddTicks(3532));

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_UpvotedUsersId",
                table: "CommentUser",
                column: "UpvotedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatorId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Parent",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpvoteCount",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Comments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 1, 10, 21, 11, 56, 215, DateTimeKind.Local).AddTicks(5688));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

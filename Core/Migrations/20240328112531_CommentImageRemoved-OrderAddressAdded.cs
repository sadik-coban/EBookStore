using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CommentImageRemovedOrderAddressAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentImages");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerAddressId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerAddressId",
                table: "Orders",
                column: "CustomerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerAddresses_CustomerAddressId",
                table: "Orders",
                column: "CustomerAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerAddresses_CustomerAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerAddressId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "CommentImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentImages_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentImages_CommentId",
                table: "CommentImages",
                column: "CommentId");
        }
    }
}

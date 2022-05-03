using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanTech.Migrations
{
    public partial class dfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

           
            
            
            migrationBuilder.CreateTable(
                name: "BasketsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    NumberOfProduct = table.Column<int>(type: "int", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketsHistory_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketsHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });




            migrationBuilder.CreateIndex(
                name: "IX_BasketsHistory_ProductId",
                table: "BasketsHistory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketsHistory_UserId",
                table: "BasketsHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketsHistory");

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Customers.Migrations
{
    public partial class AddClassificationToCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassificationId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ClassificationId",
                table: "Customer",
                column: "ClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Classification_ClassificationId",
                table: "Customer",
                column: "ClassificationId",
                principalTable: "Classification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Classification_ClassificationId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ClassificationId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "Customer");
        }
    }
}

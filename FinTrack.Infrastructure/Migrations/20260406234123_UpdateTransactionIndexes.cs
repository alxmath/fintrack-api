using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId_Description",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId_Date",
                table: "transactions",
                columns: new[] { "UserId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId_Date",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId_Description",
                table: "transactions",
                columns: new[] { "UserId", "Description" },
                unique: true);
        }
    }
}

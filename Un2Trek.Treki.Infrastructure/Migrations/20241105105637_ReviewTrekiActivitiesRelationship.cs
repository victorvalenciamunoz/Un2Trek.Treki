using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReviewTrekiActivitiesRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTrekis_Trekis_TrekiId",
                table: "ActivityTrekis");

            migrationBuilder.DropIndex(
                name: "IX_ActivityTrekis_TrekiId",
                table: "ActivityTrekis");

            migrationBuilder.CreateTable(
                name: "ActivityTrekiTreki",
                columns: table => new
                {
                    ActivityTrekiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrekiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTrekiTreki", x => new { x.ActivityTrekiId, x.TrekiId });
                    table.ForeignKey(
                        name: "FK_ActivityTrekiTreki_ActivityTrekis_ActivityTrekiId",
                        column: x => x.ActivityTrekiId,
                        principalTable: "ActivityTrekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityTrekiTreki_Trekis_TrekiId",
                        column: x => x.TrekiId,
                        principalTable: "Trekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTrekiTreki_TrekiId",
                table: "ActivityTrekiTreki",
                column: "TrekiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityTrekiTreki");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTrekis_TrekiId",
                table: "ActivityTrekis",
                column: "TrekiId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTrekis_Trekis_TrekiId",
                table: "ActivityTrekis",
                column: "TrekiId",
                principalTable: "Trekis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

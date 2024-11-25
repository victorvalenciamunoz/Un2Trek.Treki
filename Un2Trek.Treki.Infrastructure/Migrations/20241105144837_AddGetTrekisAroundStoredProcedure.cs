using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations;

public partial class AddGetTrekisAroundStoredProcedure : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[GetTrekisAround]
                @currentLatitude float,
                @currentLongitude float,
                @threshold int
            AS
            BEGIN
                DECLARE @source geography = geography::Point(@currentLatitude, @currentLongitude, 4326);

                SELECT trekis.*
                FROM trekis
                WHERE @source.STDistance(geography::Point(Latitude, Longitude, 4326)) <= @threshold
                AND trekis.IsActive = 1
            END
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetTrekisAround]");
    }
}

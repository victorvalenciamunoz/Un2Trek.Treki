using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations;

public partial class AddGetTrekisAroundStoredProcedure : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {        
        migrationBuilder.Sql(@"
            IF OBJECT_ID('dbo.GetTrekisAround', 'P') IS NOT NULL
                DROP PROCEDURE dbo.GetTrekisAround;
        ");
     
        migrationBuilder.Sql(@"
            EXEC('
                CREATE PROCEDURE [dbo].[GetTrekisAround]
                    @currentLatitude FLOAT,
                    @currentLongitude FLOAT,
                    @threshold INT
                AS
                BEGIN
                    DECLARE @source GEOGRAPHY = GEOGRAPHY::Point(@currentLatitude, @currentLongitude, 4326);

                    SELECT trekis.*
                    FROM trekis
                    WHERE @source.STDistance(GEOGRAPHY::Point(Latitude, Longitude, 4326)) <= @threshold
                      AND trekis.IsActive = 1;
                END
            ');
        ", suppressTransaction: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
               IF OBJECT_ID('dbo.GetTrekisAround', 'P') IS NOT NULL
                   DROP PROCEDURE dbo.GetTrekisAround;
           ");
    }
}

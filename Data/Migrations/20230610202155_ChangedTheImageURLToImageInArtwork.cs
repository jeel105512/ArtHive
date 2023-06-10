using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtHive.Data.Migrations
{
    public partial class ChangedTheImageURLToImageInArtwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Artworks",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Artworks",
                newName: "ImageURL");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookOnline.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_ImageInfo_ImageAuthorId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_Author_AuthorId",
                table: "BookDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_ImageInfo_ImageBookId",
                table: "BookDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageInfo",
                table: "ImageInfo");

            migrationBuilder.RenameTable(
                name: "ImageInfo",
                newName: "Images");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "BookDetails",
                newName: "GenreForCategory");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "BookDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_Images_ImageAuthorId",
                table: "Author",
                column: "ImageAuthorId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_Author_AuthorId",
                table: "BookDetails",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_Images_ImageBookId",
                table: "BookDetails",
                column: "ImageBookId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Author_Images_ImageAuthorId",
                table: "Author");

            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_Author_AuthorId",
                table: "BookDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BookDetails_Images_ImageBookId",
                table: "BookDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "ImageInfo");

            migrationBuilder.RenameColumn(
                name: "GenreForCategory",
                table: "BookDetails",
                newName: "Genre");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                table: "BookDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageInfo",
                table: "ImageInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Author_ImageInfo_ImageAuthorId",
                table: "Author",
                column: "ImageAuthorId",
                principalTable: "ImageInfo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_Author_AuthorId",
                table: "BookDetails",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookDetails_ImageInfo_ImageBookId",
                table: "BookDetails",
                column: "ImageBookId",
                principalTable: "ImageInfo",
                principalColumn: "Id");
        }
    }
}

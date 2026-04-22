using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace com.danliris.support.lib.Migrations
{
    public partial class AddTableStatusRespon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TPBStatusRespon",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    IdHeader = table.Column<long>(nullable: false),
                    _CreatedAgent = table.Column<string>(nullable: true),
                    _CreatedBy = table.Column<string>(nullable: true),
                    _CreatedUtc = table.Column<DateTime>(nullable: false),
                    _DeletedAgent = table.Column<string>(nullable: true),
                    _DeletedBy = table.Column<string>(nullable: true),
                    _DeletedUtc = table.Column<DateTime>(nullable: false),
                    _IsDeleted = table.Column<bool>(nullable: false),
                    _LastModifiedAgent = table.Column<string>(nullable: true),
                    _LastModifiedBy = table.Column<string>(nullable: true),
                    _LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    kodeDokumenPendukung = table.Column<string>(nullable: true),
                    kodeDokumenUtama = table.Column<string>(nullable: true),
                    kodeProses = table.Column<string>(nullable: true),
                    namaDokumenPendukung = table.Column<string>(nullable: true),
                    namaDokumenUtama = table.Column<string>(nullable: true),
                    nomorDokumenPendukung = table.Column<string>(nullable: true),
                    nomorDokumenUtama = table.Column<string>(nullable: true),
                    statusProses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPBStatusRespon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TPBStatusRespon_TPBHeader_IdHeader",
                        column: x => x.IdHeader,
                        principalTable: "TPBHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TPBStatusRespon_IdHeader",
                table: "TPBStatusRespon",
                column: "IdHeader");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TPBStatusRespon");
        }
    }
}

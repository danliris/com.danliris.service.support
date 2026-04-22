using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace com.danliris.support.lib.Migrations
{
    public partial class MigrationOld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "BCId",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "DetailshippingOrderId",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_CreatedAgent",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_CreatedBy",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_CreatedUtc",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_DeletedAgent",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_DeletedBy",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_DeletedUtc",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_IsDeleted",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_LastModifiedAgent",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_LastModifiedBy",
                table: "ViewFactBeacukai");

            migrationBuilder.DropColumn(
                name: "_LastModifiedUtc",
                table: "ViewFactBeacukai");

            migrationBuilder.AlterColumn<string>(
                name: "Quantity",
                table: "ViewFactBeacukai",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "Nominal",
                table: "ViewFactBeacukai",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ViewFactBeacukai",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "ViewFactBeacukai",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Nominal",
                table: "ViewFactBeacukai",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ViewFactBeacukai",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ViewFactBeacukai",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BCId",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailshippingOrderId",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_CreatedAgent",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_CreatedBy",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "_CreatedUtc",
                table: "ViewFactBeacukai",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "_DeletedAgent",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_DeletedBy",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "_DeletedUtc",
                table: "ViewFactBeacukai",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "_IsDeleted",
                table: "ViewFactBeacukai",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedAgent",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "_LastModifiedBy",
                table: "ViewFactBeacukai",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "_LastModifiedUtc",
                table: "ViewFactBeacukai",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

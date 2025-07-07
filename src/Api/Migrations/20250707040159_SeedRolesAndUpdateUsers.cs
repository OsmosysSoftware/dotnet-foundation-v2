using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations;

/// <inheritdoc />
public partial class SeedRolesAndUpdateUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "password_hash",
            table: "users",
            type: "varchar(255)",
            maxLength: 255,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "longtext")
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        // Seed initial roles
        // Assign 'User' role to users without a role
        // Set inactive users to active
        migrationBuilder.Sql(@"
            INSERT INTO roles (name, created_at, updated_at, status)
            SELECT 'Admin', NOW(), NOW(), true
            WHERE NOT EXISTS (SELECT 1 FROM roles WHERE name = 'Admin');

            INSERT INTO roles (name, created_at, updated_at, status)
            SELECT 'User', NOW(), NOW(), true
            WHERE NOT EXISTS (SELECT 1 FROM roles WHERE name = 'User');

            UPDATE users 
            SET role_id = (SELECT id FROM roles WHERE name = 'User' LIMIT 1)
            WHERE role_id IS NULL;

            UPDATE users 
            SET status = true 
            WHERE status = false;
        ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "password_hash",
            table: "users",
            type: "longtext",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(255)",
            oldMaxLength: 255)
            .Annotation("MySql:CharSet", "utf8mb4")
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        // Optionally revert seeded roles
        migrationBuilder.Sql("DELETE FROM roles WHERE name IN ('Admin', 'User');");

        // Revert updated user data if needed
        // Note: irreversible unless backup strategy is defined
    }
}

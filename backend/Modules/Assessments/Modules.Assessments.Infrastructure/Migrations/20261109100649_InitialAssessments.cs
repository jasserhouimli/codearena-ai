using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Assessments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialAssessments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assessment_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    candidate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    started_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    score = table.Column<double>(type: "double precision", nullable: true),
                    feedback = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    metadata_json = table.Column<string>(type: "jsonb", nullable: true),
                    keystroke_logs_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assessment_sessions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assessment_sessions_application_id",
                table: "assessment_sessions",
                column: "application_id");

            migrationBuilder.CreateIndex(
                name: "ix_assessment_sessions_candidate_id",
                table: "assessment_sessions",
                column: "candidate_id");

            migrationBuilder.CreateIndex(
                name: "ix_assessment_sessions_status",
                table: "assessment_sessions",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assessment_sessions");
        }
    }
}

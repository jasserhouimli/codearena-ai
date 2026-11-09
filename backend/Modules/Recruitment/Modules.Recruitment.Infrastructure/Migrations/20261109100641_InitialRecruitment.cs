using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Recruitment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialRecruitment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    recruiter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    company_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    requirements = table.Column<string>(type: "text", nullable: false),
                    tech_stack = table.Column<List<string>>(type: "text[]", nullable: false),
                    salary_min = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    salary_max = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "Remote"),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_posts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    candidate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_stage = table.Column<int>(type: "integer", nullable: false),
                    overall_score = table.Column<double>(type: "double precision", nullable: true),
                    resume_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applications", x => x.id);
                    table.ForeignKey(
                        name: "fk_applications_job_posts_job_post_id",
                        column: x => x.job_post_id,
                        principalTable: "job_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_applications_current_stage",
                table: "applications",
                column: "current_stage");

            migrationBuilder.CreateIndex(
                name: "ix_applications_job_post_id_candidate_id",
                table: "applications",
                columns: new[] { "job_post_id", "candidate_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_job_posts_recruiter_id",
                table: "job_posts",
                column: "recruiter_id");

            migrationBuilder.CreateIndex(
                name: "ix_job_posts_status",
                table: "job_posts",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "job_posts");
        }
    }
}

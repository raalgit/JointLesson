using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202202152125)]
    public class UpdateLastPageType : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Delete.Column("LastMaterialPage").FromTable("GroupAtCourse").InSchema("JL");
            Create.Column("LastMaterialPage").OnTable("GroupAtCourse").InSchema("JL").AsString(40).Nullable();

            Delete.Column("LastMaterialPage").FromTable("Lesson").InSchema("JL");
            Create.Column("LastMaterialPage").OnTable("Lesson").InSchema("JL").AsString(40).Nullable();
        }
    }
}

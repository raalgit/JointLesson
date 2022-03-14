using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203132025)]
    public class AlterCourseTeacherAddColumn : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("CourseTeacher").InSchema("JL")
                .AddColumn("OnLesson").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }
    }
}

using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203081241)]
    public class AddCourseIdAtLesson : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("Lesson").InSchema("JL")
                .AddColumn("CourseId")
                .AsInt32()
                .ForeignKey("FK_LessonCourseId", "JL", "Course", "Id")
                .NotNullable()
                .SetExistingRowsTo("3");
        }
    }
}

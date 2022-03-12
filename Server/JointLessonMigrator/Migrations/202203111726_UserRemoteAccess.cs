using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203111727)]
    public class UserRemoteAccess : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("UserRemoteAccess").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("CourseId").AsInt32().ForeignKey("FK_UserRemoteAccessCourseId", "JL", "Course", "Id").NotNullable()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_UserRemoteAccessUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("ConnectionData").AsString(2000).NotNullable()
                .WithColumn("StartDate").AsDateTime().NotNullable();
        }
    }
}

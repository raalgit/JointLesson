using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202202111905)]
    public class AlterFileDataColumn : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Delete.Column("FileId").FromTable("Manual").InSchema("JL");
            Create.Column("FileDataId").OnTable("Manual").InSchema("JL")
                .AsInt32().ForeignKey("FK_ManualFileDataId", "JL", "FileData", "Id").Nullable();

            Delete.Column("AvatarId").FromTable("User").InSchema("JL");
            Create.Column("AvatarId").OnTable("User").InSchema("JL")
                .AsInt32().ForeignKey("FK_UserAvatarIdId", "JL", "FileData", "Id").Nullable();

            Delete.Column("AvatarId").FromTable("Course").InSchema("JL");
            Create.Column("AvatarId").OnTable("Course").InSchema("JL")
                .AsInt32().ForeignKey("FK_CourseAvatarIdId", "JL", "FileData", "Id").Nullable();
        }
    }
}

using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202202110115)]
    public class AddFileData : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("FileData").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("MongoName").AsString(30).NotNullable()
                .WithColumn("OriginalName").AsString(50).NotNullable()
                .WithColumn("MongoId").AsString(100).NotNullable();

            Delete.ForeignKey("FK_UserGroupId").OnTable("User").InSchema("JL");

            Alter.Column("GroupId").OnTable("User").InSchema("JL")
                .AsInt32().ForeignKey("FK_UserGroupId", "JL", "Group", "Id").Nullable();
        }
    }
}

using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203211835)]
    public class AlterNoteTable : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            Create.Column("FileDataId").OnTable("WorkBook").InSchema("JL")
               .AsInt32().ForeignKey("FK_WorkBookFileDataId", "JL", "FileData", "Id").Nullable();

            Delete.Column("Text").FromTable("WorkBook").InSchema("JL");
            Delete.Column("Page").FromTable("WorkBook").InSchema("JL");

            Create.Column("Page").OnTable("WorkBook").InSchema("JL").AsString(40).Nullable();

            Delete.ForeignKey("FK_WorkBookLessonId").OnTable("WorkBook").InSchema("JL");
            Delete.Column("LessonId").FromTable("WorkBook").InSchema("JL");
        }
    }
}

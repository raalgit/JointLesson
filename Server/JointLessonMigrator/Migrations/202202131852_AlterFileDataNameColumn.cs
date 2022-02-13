using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202202141852)]
    public class AlterFileDataNameColumn : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Column("MongoName").OnTable("FileData").InSchema("JL")
                .AsString(50).NotNullable();
        }
    }
}

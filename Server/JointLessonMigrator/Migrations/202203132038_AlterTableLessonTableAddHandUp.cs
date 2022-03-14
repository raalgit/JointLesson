using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203132038)]
    public class AlterTableLessonTableAddHandUp : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Alter.Table("LessonTabel").InSchema("JL")
                .AddColumn("HandUp").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }
    }
}

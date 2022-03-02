using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.JointLessonMigrator.Migrations
{
    [Migration(202203021505)]
    public class CreateSignalConnection : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("SignalUserConnection").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_SignalUserConnectionUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("ConnectionId").AsString(50).NotNullable();
        }
    }
}

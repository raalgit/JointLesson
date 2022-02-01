using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonMigrator.Migrations
{
    [Migration(202202011636)]
    public class TestMigration : Migration
    {
        public override void Down()
        {
            Delete.FromTable("TestTable").InSchema("Test");
            Delete.Table("TestTable").InSchema("Test");
            Delete.Schema("Test");
        }

        public override void Up()
        {
            Create.Schema("Test");

            Create.Table("TestTable").InSchema("Test")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().WithColumnDescription("Номер записи")
                .WithColumn("Name").AsString(50).NotNullable().WithColumnDescription("Имя");

            Insert.IntoTable("TestTable").InSchema("Test")
                .Row(new
                {
                    Id = 0,
                    Name = "Новая запись"
                });
        }
    }
}

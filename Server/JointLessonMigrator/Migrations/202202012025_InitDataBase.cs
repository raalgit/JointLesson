using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointLessonMigrator.Migrations
{
    [Migration(202202012026)]
    public class InitDataBase : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Create.Schema("JL");

            Create.Table("Group").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable();

            Create.Table("User").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("FirstName").AsString(50).NotNullable()
                .WithColumn("SecondName").AsString(50).NotNullable()
                .WithColumn("ThirdName").AsString(50).Nullable()
                .WithColumn("GroupId").AsInt32().ForeignKey("FK_UserGroupId", "JL", "Group", "Id").NotNullable()
                .WithColumn("AvatarId").AsString(50).Nullable();

            Create.Table("Role").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("SystemName").AsString(50).NotNullable()
                .WithColumn("DisplayName").AsString(50).NotNullable();

            Create.Table("UserRole").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_UserRoleUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("RoleId").AsInt32().ForeignKey("FK_UserRoleRoleId", "JL", "Role", "Id").NotNullable();

            Create.Table("Statistic").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_StatisticUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("SuccessExecution").AsInt32().NotNullable()
                .WithColumn("FailedExecution").AsInt32().NotNullable();

            Create.Table("AuthData").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_AuthDataUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("Login").AsString(20).NotNullable()
                .WithColumn("PasswordHash").AsString(50).NotNullable();

            Create.Table("Manual").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("FileId").AsString(50).NotNullable()
                .WithColumn("AuthorId").AsInt32().ForeignKey("FK_ManualAuthorId", "JL", "User", "Id").NotNullable()
                .WithColumn("Title").AsString(50).NotNullable();

            Create.Table("Discipline").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(50).NotNullable();

            Create.Table("Course").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("CreatorId").AsInt32().ForeignKey("FK_CourseCreatorId", "JL", "User", "Id").NotNullable()
                .WithColumn("AvatarId").AsString(50).Nullable()
                .WithColumn("ManualId").AsInt32().ForeignKey("FK_CourseManualId", "JL", "Manual", "Id").NotNullable()
                .WithColumn("DisciplineId").AsInt32().ForeignKey("FK_CourseDisciplineId", "JL", "Discipline", "Id").NotNullable();

            Create.Table("CourseTeacher").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_CourseTeacherUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("CourseId").AsInt32().ForeignKey("FK_CourseTeacherCourseId", "JL", "Course", "Id").NotNullable();

            Create.Table("GroupAtCourse").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("GroupId").AsInt32().ForeignKey("FK_GroupAtCourseGroupId", "JL", "Group", "Id").NotNullable()
                .WithColumn("CourseId").AsInt32().ForeignKey("FK_GroupAtCourseCourseId", "JL", "Course", "Id").NotNullable()
                .WithColumn("LastMaterialPage").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(false);

            Create.Table("Lesson").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("GroupAtCourseId").AsInt32().ForeignKey("FK_LessonGroupAtCourseId", "JL", "GroupAtCourse", "Id").NotNullable()
                .WithColumn("LastMaterialPage").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("StartDate").AsDateTime().NotNullable()
                .WithColumn("EndDate").AsDateTime().Nullable()
                .WithColumn("TeacherId").AsInt32().ForeignKey("FK_LessonTeacherId", "JL", "User", "Id").NotNullable()
                .WithColumn("Type").AsString(10).NotNullable().WithDefaultValue("ONLINE");

            Create.Table("WorkBook").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_WorkBookUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("LessonId").AsInt32().ForeignKey("FK_WorkBookLessonId", "JL", "Lesson", "Id").NotNullable()
                .WithColumn("Text").AsString(400).Nullable()
                .WithColumn("Page").AsInt32().NotNullable();

            Create.Table("Test").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_TestUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("LessonId").AsInt32().ForeignKey("FK_TestLessonId", "JL", "Lesson", "Id").NotNullable()
                .WithColumn("BlockId").AsInt32().Nullable()
                .WithColumn("SendDate").AsDateTime().NotNullable()
                .WithColumn("ResultId").AsString(50).Nullable();

            Create.Table("LessonTabel").InSchema("JL")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt32().ForeignKey("FK_LessonTabelUserId", "JL", "User", "Id").NotNullable()
                .WithColumn("LessonId").AsInt32().ForeignKey("FK_LessonTabelLessonId", "JL", "Lesson", "Id").NotNullable()
                .WithColumn("EnterDate").AsDateTime().NotNullable()
                .WithColumn("LeaveDate").AsDateTime().Nullable();



            Insert.IntoTable("Role").InSchema("JL")
                .Row(new
                {
                    SystemName = "User",
                    DisplayName = "Пользователь"
                })
                .Row(new
                {
                    SystemName = "Student",
                    DisplayName = "Студент"
                })
                .Row(new
                {
                    SystemName = "Teacher",
                    DisplayName = "Преподаватель"
                })
                .Row(new
                {
                    SystemName = "Editor",
                    DisplayName = "Редактор"
                })
                .Row(new
                {
                    SystemName = "Admin",
                    DisplayName = "Администратор"
                });
        }
    }
}

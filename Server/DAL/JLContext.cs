using JL.Persist;
using JL.PersistModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL
{
    public class JLContext : DbContext
    {
        public JLContext(DbContextOptions<JLContext> options) : base(options)
        {
        }

        public DbSet<AuthData> AuthDatas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAtCourse> GroupAtCourses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonTabel> LessonTabels { get; set; }
        public DbSet<Manual> Manuals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<WorkBook> WorkBooks { get; set; }
        public DbSet<FileData> FileDatas { get; set; }
        public DbSet<SignalUserConnection> SignalUserConnections { get; set; }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TestDISC.Database.TestDISCModels;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestDISC.Database
{
    public partial class TestDISCContext : DbContext
    {
        public TestDISCContext()
        {
        }

        public TestDISCContext(DbContextOptions<TestDISCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Logaction> Logaction { get; set; }
        public virtual DbSet<Loginuser> Loginuser { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Questiondetail> Questiondetail { get; set; }
        public virtual DbSet<Questiongroup> Questiongroup { get; set; }
        public virtual DbSet<Questiongroupdetail> Questiongroupdetail { get; set; }
        public virtual DbSet<Refreshtoken> Refreshtoken { get; set; }
        public virtual DbSet<Resultdisc> Resultdisc { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Useranswer> Useranswer { get; set; }
        public virtual DbSet<Useranswerquestion> Useranswerquestion { get; set; }
        public virtual DbSet<Useranswerquestiondetail> Useranswerquestiondetail { get; set; }
        public virtual DbSet<Usertitle> Usertitle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=14.241.225.51;port=3473;database=testdisc;user=testdisc_dev;password=DISC@2022", x => x.ServerVersion("8.0.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logaction>(entity =>
            {
                entity.ToTable("logaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("ipaddress")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("json");
            });

            modelBuilder.Entity<Loginuser>(entity =>
            {
                entity.ToTable("loginuser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Partnerid).HasColumnName("partnerid");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("partner");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Questiondetail>(entity =>
            {
                entity.ToTable("questiondetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Leastpoint).HasColumnName("leastpoint");

                entity.Property(e => e.Mostpoint).HasColumnName("mostpoint");

                entity.Property(e => e.Orderview).HasColumnName("orderview");

                entity.Property(e => e.Questionid).HasColumnName("questionid");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Questiongroup>(entity =>
            {
                entity.ToTable("questiongroup");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Suggest)
                    .HasColumnName("suggest")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Tutorial)
                    .HasColumnName("tutorial")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Questiongroupdetail>(entity =>
            {
                entity.ToTable("questiongroupdetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Orderview).HasColumnName("orderview");

                entity.Property(e => e.Questiongroupid).HasColumnName("questiongroupid");

                entity.Property(e => e.Questionid).HasColumnName("questionid");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Refreshtoken>(entity =>
            {
                entity.ToTable("refreshtoken");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accesstoken)
                    .HasColumnName("accesstoken")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Expiration)
                    .HasColumnName("expiration")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("ipaddress")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Updatedate)
                    .HasColumnName("updatedate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Used).HasColumnName("used");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Resultdisc>(entity =>
            {
                entity.ToTable("resultdisc");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Codetext)
                    .HasColumnName("codetext")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Interpret)
                    .HasColumnName("interpret")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Quanlyti)
                    .HasColumnName("quanlyti")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Jobposition)
                    .HasColumnName("jobposition")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Namecompany)
                    .HasColumnName("namecompany")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Partnerid).HasColumnName("partnerid");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.Updatedate)
                    .HasColumnName("updatedate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Useranswer>(entity =>
            {
                entity.ToTable("useranswer");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Partnerid).HasColumnName("partnerid");

                entity.Property(e => e.Questiongroupid).HasColumnName("questiongroupid");

                entity.Property(e => e.Resultdiscid).HasColumnName("resultdiscid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Useranswerquestion>(entity =>
            {
                entity.ToTable("useranswerquestion");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Questionid).HasColumnName("questionid");

                entity.Property(e => e.Useranswerid).HasColumnName("useranswerid");
            });

            modelBuilder.Entity<Useranswerquestiondetail>(entity =>
            {
                entity.ToTable("useranswerquestiondetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Leastquestiondetailid).HasColumnName("leastquestiondetailid");

                entity.Property(e => e.Mostquestiondetailid).HasColumnName("mostquestiondetailid");

                entity.Property(e => e.Questionid).HasColumnName("questionid");

                entity.Property(e => e.Useranswerquestionid).HasColumnName("useranswerquestionid");
            });

            modelBuilder.Entity<Usertitle>(entity =>
            {
                entity.ToTable("usertitle");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

﻿// <auto-generated />
using System;
using AlpimiAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace alpimi_planner_backend.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241125192644_AddedTeacher")]
    partial class AddedTeacher
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AlpimiAPI.Entities.EAuth.Auth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Auth");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EDayOff.DayOff", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("From")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ScheduleSettingsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("To")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleSettingsId");

                    b.ToTable("DayOff");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ELessonPerioid.LessonPeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("Finish")
                        .HasColumnType("time");

                    b.Property<Guid>("ScheduleSettingsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleSettingsId");

                    b.ToTable("LessonPeriod");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ESchedule.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EScheduleSettings.ScheduleSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SchoolHour")
                        .HasColumnType("int");

                    b.Property<DateOnly>("SchoolYearEnd")
                        .HasColumnType("date");

                    b.Property<DateOnly>("SchoolYearStart")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("ScheduleSettings");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ETeacher.Teacher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EUser.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CustomURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EAuth.Auth", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.EUser.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EDayOff.DayOff", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.EScheduleSettings.ScheduleSettings", "ScheduleSettings")
                        .WithMany()
                        .HasForeignKey("ScheduleSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScheduleSettings");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ELessonPerioid.LessonPeriod", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.EScheduleSettings.ScheduleSettings", "ScheduleSettings")
                        .WithMany()
                        .HasForeignKey("ScheduleSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScheduleSettings");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ESchedule.Schedule", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.EUser.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.EScheduleSettings.ScheduleSettings", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.ESchedule.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("AlpimiAPI.Entities.ETeacher.Teacher", b =>
                {
                    b.HasOne("AlpimiAPI.Entities.ESchedule.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });
#pragma warning restore 612, 618
        }
    }
}

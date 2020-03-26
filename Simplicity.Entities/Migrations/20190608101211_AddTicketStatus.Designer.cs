﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Simplicity.Entities;

namespace Simplicity.Entities.Migrations
{
    [DbContext(typeof(SimplicityContext))]
    [Migration("20190608101211_AddTicketStatus")]
    partial class AddTicketStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Simplicity.Entities.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("FromDate");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("ToDate");

                    b.HasKey("ID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Simplicity.Entities.Ticket", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssigneeID");

                    b.Property<int>("CreatorID");

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectID");

                    b.Property<int>("Status");

                    b.HasKey("ID");

                    b.HasIndex("AssigneeID");

                    b.HasIndex("CreatorID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Simplicity.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime?>("Birthday");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("PicturePath");

                    b.Property<int>("Role");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Simplicity.Entities.UserProject", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProjectID");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UserID");

                    b.ToTable("UsersProjects");
                });

            modelBuilder.Entity("Simplicity.Entities.Ticket", b =>
                {
                    b.HasOne("Simplicity.Entities.User", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Simplicity.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Simplicity.Entities.Project", "Project")
                        .WithMany("Tickets")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Simplicity.Entities.UserProject", b =>
                {
                    b.HasOne("Simplicity.Entities.Project", "Project")
                        .WithMany("UsersProjects")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Simplicity.Entities.User", "User")
                        .WithMany("UsersProjects")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
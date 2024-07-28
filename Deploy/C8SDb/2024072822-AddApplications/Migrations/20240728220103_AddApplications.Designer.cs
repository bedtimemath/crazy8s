﻿// <auto-generated />
using System;
using C8S.Database.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace C8S.Database.EFCore.Migrations
{
    [DbContext(typeof(C8SDbContext))]
    [Migration("20240728220103_AddApplications")]
    partial class AddApplications
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationDb", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<string>("ApplicantEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantFirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantLastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantPhone")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ApplicantPhoneExt")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ApplicantTimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ApplicantType")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Comments")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LinkedCoachId")
                        .HasColumnType("int");

                    b.Property<int?>("LinkedOrganizationId")
                        .HasColumnType("int");

                    b.Property<Guid?>("OldSystemApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OldSystemNotes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganizationName")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("OrganizationTaxIdentifier")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OrganizationType")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("OrganizationTypeOther")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTimeOffset>("SubmittedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("WorkshopCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ApplicationId");

                    b.HasIndex("LinkedCoachId");

                    b.HasIndex("LinkedOrganizationId");

                    b.HasIndex("OldSystemApplicationId")
                        .IsUnique()
                        .HasFilter("[OldSystemApplicationId] IS NOT NULL");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.CoachDb", b =>
                {
                    b.Property<int>("CoachId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CoachId"));

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid?>("OldSystemCoachId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemCompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OldSystemNotes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("PhoneExt")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CoachId");

                    b.HasIndex("OldSystemCoachId")
                        .IsUnique()
                        .HasFilter("[OldSystemCoachId] IS NOT NULL");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrganizationDb", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganizationId"));

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<Guid?>("OldSystemCompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OldSystemNotes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TaxIdentifier")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("TypeOther")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("OrganizationId");

                    b.HasIndex("OldSystemOrganizationId")
                        .IsUnique()
                        .HasFilter("[OldSystemOrganizationId] IS NOT NULL");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.CoachDb", "LinkedCoach")
                        .WithMany("Applications")
                        .HasForeignKey("LinkedCoachId");

                    b.HasOne("C8S.Database.EFCore.Models.OrganizationDb", "LinkedOrganization")
                        .WithMany("Applications")
                        .HasForeignKey("LinkedOrganizationId");

                    b.Navigation("LinkedCoach");

                    b.Navigation("LinkedOrganization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.CoachDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.OrganizationDb", "Organization")
                        .WithMany("Coaches")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.CoachDb", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrganizationDb", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Coaches");
                });
#pragma warning restore 612, 618
        }
    }
}

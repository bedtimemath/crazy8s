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
    [Migration("20240805173023_AddOrderSkuTable2")]
    partial class AddOrderSkuTable2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("C8S.Database.EFCore.Models.AddressDb", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("BusinessName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsMilitary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("OldSystemUsaPostalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("RecipientName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AddressId");

                    b.HasIndex("OldSystemUsaPostalId")
                        .IsUnique()
                        .HasFilter("[OldSystemUsaPostalId] IS NOT NULL");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationClubDb", b =>
                {
                    b.Property<int>("ApplicationClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationClubId"));

                    b.Property<string>("AgeLevel")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<string>("ClubSize")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("OldSystemApplicationClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemLinkedClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<DateOnly>("StartsOn")
                        .HasColumnType("date");

                    b.HasKey("ApplicationClubId");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("OldSystemApplicationClubId")
                        .IsUnique()
                        .HasFilter("[OldSystemApplicationClubId] IS NOT NULL");

                    b.ToTable("ApplicationClubs");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationDb", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("ApplicantEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantFirstName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantLastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ApplicantPhone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ApplicantPhoneExt")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ApplicantTimeZone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ApplicantType")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Comments")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsCoachRemoved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsOrganizationRemoved")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("LinkedCoachId")
                        .HasColumnType("int");

                    b.Property<int?>("LinkedOrganizationId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemApplicationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemLinkedCoachId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemLinkedOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("OrganizationTaxIdentifier")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("OrganizationType")
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

                    b.HasIndex("AddressId")
                        .IsUnique()
                        .HasFilter("[AddressId] IS NOT NULL");

                    b.HasIndex("LinkedCoachId");

                    b.HasIndex("LinkedOrganizationId");

                    b.HasIndex("OldSystemApplicationId")
                        .IsUnique()
                        .HasFilter("[OldSystemApplicationId] IS NOT NULL");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ClubDb", b =>
                {
                    b.Property<int>("ClubId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClubId"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("AgeLevel")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ClubSize")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("CoachId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemCoachId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemMeetingAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<DateOnly>("StartsOn")
                        .HasColumnType("date");

                    b.HasKey("ClubId");

                    b.HasIndex("AddressId")
                        .IsUnique()
                        .HasFilter("[AddressId] IS NOT NULL");

                    b.HasIndex("CoachId");

                    b.HasIndex("OldSystemClubId")
                        .IsUnique()
                        .HasFilter("[OldSystemClubId] IS NOT NULL");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Clubs");
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

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemCoachId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemCompanyId")
                        .HasColumnType("uniqueidentifier");

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

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrderDb", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("ArriveBy")
                        .HasColumnType("date");

                    b.Property<Guid?>("BatchIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ClubId")
                        .HasColumnType("int");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ContactPhone")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ContactPhoneExt")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("EmailedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<Guid?>("OldSystemClubId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemOrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemShippingAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("OrderedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ShippedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("OrderId");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ClubId");

                    b.HasIndex("OldSystemOrderId")
                        .IsUnique()
                        .HasFilter("[OldSystemOrderId] IS NOT NULL");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrderSkuDb", b =>
                {
                    b.Property<int>("OrderSkuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderSkuId"));

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("OldSystemOrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemOrderSkuId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemSkuId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Ordinal")
                        .HasColumnType("int");

                    b.Property<short>("Quantity")
                        .HasColumnType("smallint");

                    b.Property<int>("SkuId")
                        .HasColumnType("int");

                    b.HasKey("OrderSkuId");

                    b.HasIndex("OldSystemOrderSkuId")
                        .IsUnique()
                        .HasFilter("[OldSystemOrderSkuId] IS NOT NULL");

                    b.HasIndex("OrderId");

                    b.HasIndex("SkuId");

                    b.ToTable("OrderSkus");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrganizationDb", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganizationId"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

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

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemCompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("OldSystemPostalAddressId")
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

                    b.HasIndex("AddressId")
                        .IsUnique()
                        .HasFilter("[AddressId] IS NOT NULL");

                    b.HasIndex("OldSystemOrganizationId")
                        .IsUnique()
                        .HasFilter("[OldSystemOrganizationId] IS NOT NULL");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.SkuDb", b =>
                {
                    b.Property<int>("SkuId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkuId"));

                    b.Property<string>("AgeLevel")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ClubSize")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Notes")
                        .HasMaxLength(4096)
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OldSystemSkuId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Season")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("SkuId");

                    b.HasIndex("OldSystemSkuId")
                        .IsUnique()
                        .HasFilter("[OldSystemSkuId] IS NOT NULL");

                    b.ToTable("Skus");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationClubDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.ApplicationDb", "Application")
                        .WithMany("ApplicationClubs")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.AddressDb", "Address")
                        .WithOne("Application")
                        .HasForeignKey("C8S.Database.EFCore.Models.ApplicationDb", "AddressId");

                    b.HasOne("C8S.Database.EFCore.Models.CoachDb", "LinkedCoach")
                        .WithMany("Applications")
                        .HasForeignKey("LinkedCoachId");

                    b.HasOne("C8S.Database.EFCore.Models.OrganizationDb", "LinkedOrganization")
                        .WithMany("Applications")
                        .HasForeignKey("LinkedOrganizationId");

                    b.Navigation("Address");

                    b.Navigation("LinkedCoach");

                    b.Navigation("LinkedOrganization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ClubDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.AddressDb", "Address")
                        .WithOne("Club")
                        .HasForeignKey("C8S.Database.EFCore.Models.ClubDb", "AddressId");

                    b.HasOne("C8S.Database.EFCore.Models.CoachDb", "Coach")
                        .WithMany("Clubs")
                        .HasForeignKey("CoachId");

                    b.HasOne("C8S.Database.EFCore.Models.OrganizationDb", "Organization")
                        .WithMany("Clubs")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Coach");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.CoachDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.OrganizationDb", "Organization")
                        .WithMany("Coaches")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrderDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.AddressDb", "Address")
                        .WithOne("Order")
                        .HasForeignKey("C8S.Database.EFCore.Models.OrderDb", "AddressId");

                    b.HasOne("C8S.Database.EFCore.Models.ClubDb", "Club")
                        .WithMany("Orders")
                        .HasForeignKey("ClubId");

                    b.Navigation("Address");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrderSkuDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.OrderDb", "Order")
                        .WithMany("OrderSkus")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("C8S.Database.EFCore.Models.SkuDb", "Sku")
                        .WithMany("OrderSkus")
                        .HasForeignKey("SkuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Sku");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrganizationDb", b =>
                {
                    b.HasOne("C8S.Database.EFCore.Models.AddressDb", "Address")
                        .WithOne("Organization")
                        .HasForeignKey("C8S.Database.EFCore.Models.OrganizationDb", "AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.AddressDb", b =>
                {
                    b.Navigation("Application");

                    b.Navigation("Club");

                    b.Navigation("Order");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ApplicationDb", b =>
                {
                    b.Navigation("ApplicationClubs");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.ClubDb", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.CoachDb", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Clubs");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrderDb", b =>
                {
                    b.Navigation("OrderSkus");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.OrganizationDb", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Clubs");

                    b.Navigation("Coaches");
                });

            modelBuilder.Entity("C8S.Database.EFCore.Models.SkuDb", b =>
                {
                    b.Navigation("OrderSkus");
                });
#pragma warning restore 612, 618
        }
    }
}

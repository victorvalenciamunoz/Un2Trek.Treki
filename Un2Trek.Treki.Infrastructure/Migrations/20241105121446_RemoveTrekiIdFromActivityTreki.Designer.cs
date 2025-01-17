﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Un2Trek.Trekis.Infrastructure;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241105121446_RemoveTrekiIdFromActivityTreki")]
    partial class RemoveTrekiIdFromActivityTreki
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Un2Trek.Trekis.Domain.ActivityTreki", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ValidFromDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("ActivityTrekis");
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.ActivityTrekiTreki", b =>
                {
                    b.Property<Guid>("ActivityTrekiId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TrekiId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ActivityTrekiId", "TrekiId");

                    b.HasIndex("TrekiId");

                    b.ToTable("ActivityTrekiTreki");
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.Treki", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Trekis");
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.ActivityTrekiTreki", b =>
                {
                    b.HasOne("Un2Trek.Trekis.Domain.ActivityTreki", "ActivityTreki")
                        .WithMany("ActivityTrekiTrekis")
                        .HasForeignKey("ActivityTrekiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Un2Trek.Trekis.Domain.Treki", "Treki")
                        .WithMany("ActivityTrekiTrekis")
                        .HasForeignKey("TrekiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActivityTreki");

                    b.Navigation("Treki");
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.Treki", b =>
                {
                    b.OwnsOne("Un2Trek.Trekis.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("TrekiId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("TrekiId");

                            b1.ToTable("Trekis");

                            b1.WithOwner()
                                .HasForeignKey("TrekiId");
                        });

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.ActivityTreki", b =>
                {
                    b.Navigation("ActivityTrekiTrekis");
                });

            modelBuilder.Entity("Un2Trek.Trekis.Domain.Treki", b =>
                {
                    b.Navigation("ActivityTrekiTrekis");
                });
#pragma warning restore 612, 618
        }
    }
}

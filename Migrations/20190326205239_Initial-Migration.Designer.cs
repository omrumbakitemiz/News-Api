﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using News.Api;

namespace News.Api.Migrations
{
    [DbContext(typeof(NewsDbContext))]
    [Migration("20190326205239_Initial-Migration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("News.Api.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Data");

                    b.Property<string>("NewsId");

                    b.HasKey("Id");

                    b.HasIndex("NewsId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("News.Api.Models.News", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DislikeCount");

                    b.Property<int>("LikeCount");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<int>("ViewCount");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("News.Api.Models.Image", b =>
                {
                    b.HasOne("News.Api.Models.News")
                        .WithMany("Images")
                        .HasForeignKey("NewsId");
                });
#pragma warning restore 612, 618
        }
    }
}

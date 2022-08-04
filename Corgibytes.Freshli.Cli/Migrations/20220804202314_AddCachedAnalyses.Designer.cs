﻿// <auto-generated />
using System;
using Corgibytes.Freshli.Cli.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Corgibytes.Freshli.Cli.Migrations
{
    [DbContext(typeof(CacheContext))]
    [Migration("20220804202314_AddCachedAnalyses")]
    partial class AddCachedAnalyses
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-preview.6.22329.4");

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedAnalysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("HistoryInterval")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RepositoryBranch")
                        .HasColumnType("TEXT");

                    b.Property<string>("RepositoryUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CachedAnalyses");
                });

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedGitSource", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Branch")
                        .HasColumnType("TEXT");

                    b.Property<string>("LocalPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CachedGitSources");
                });

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("CachedProperties");
                });
#pragma warning restore 612, 618
        }
    }
}

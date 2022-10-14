﻿// <auto-generated />
using System;
using Corgibytes.Freshli.Cli.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Corgibytes.Freshli.Cli.Migrations
{
    [DbContext(typeof(CacheContext))]
    partial class CacheContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-rc.1.22426.7");

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedAnalysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ApiAnalysisId")
                        .HasColumnType("TEXT");

                    b.Property<string>("HistoryInterval")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RepositoryBranch")
                        .HasColumnType("TEXT");

                    b.Property<string>("RepositoryUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RevisionHistoryMode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UseCommitHistory")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedHistoryStopPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("AsOfDateTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CachedAnalysisId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GitCommitId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LocalPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RepositoryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CachedAnalysisId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("CachedHistoryStopPoints");
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

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedHistoryStopPoint", b =>
                {
                    b.HasOne("Corgibytes.Freshli.Cli.DataModel.CachedAnalysis", "CachedAnalysis")
                        .WithMany("HistoryStopPoints")
                        .HasForeignKey("CachedAnalysisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CachedAnalysis");
                });

            modelBuilder.Entity("Corgibytes.Freshli.Cli.DataModel.CachedAnalysis", b =>
                {
                    b.Navigation("HistoryStopPoints");
                });
#pragma warning restore 612, 618
        }
    }
}

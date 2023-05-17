using System;
using System.Collections.Generic;
using ClinicaBase.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicaBase.Data;

public partial class ClinicaBase1Context : DbContext
{
    public ClinicaBase1Context()
    {
    }

    public ClinicaBase1Context(DbContextOptions<ClinicaBase1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Control> Controls { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Control>(entity =>
        {
            entity.HasOne(d => d.Patient).WithMany(p => p.Controls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Controls_Patients");

            entity.HasOne(d => d.User).WithMany(p => p.Controls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Controls_Users");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Documento).HasName("PK_PatientsInfo");

            entity.Property(e => e.Documento).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Patients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patients_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Documento).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

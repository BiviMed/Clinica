using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicaBase.Models.Entities;

public partial class Patient
{
    [Key]
    public int Documento { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombres { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Apellidos { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Correo { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Direccion { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime FechaNacimiento { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Profesion { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Ocupacion { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string TelefonoFamiliar { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string NombreFamiliar { get; set; } = null!;

    [Column("TipoEPS")]
    [StringLength(30)]
    [Unicode(false)]
    public string TipoEps { get; set; } = null!;

    [Column("NombreEPS")]
    [StringLength(50)]
    [Unicode(false)]
    public string NombreEps { get; set; } = null!;

    [Column(TypeName = "text")]
    public string ExamenFisico { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Antecedentes { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? AntecedentesFarmac { get; set; }

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaCreacion { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<Control> Controls { get; set; } = new List<Control>();

    [ForeignKey("UserId")]
    [InverseProperty("Patients")]
    public virtual User User { get; set; } = null!;
}

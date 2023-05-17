using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicaBase.Models.Entities;

[Index("Documento", Name = "IX_Users", IsUnique = true)]
public partial class User
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
    public string Correo { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [Unicode(false)]
    public string Contrasena { get; set; } = null!;

    public byte CambioContrasena { get; set; }

    [Unicode(false)]
    public string Sal { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Rol { get; set; } = null!;

    public byte Activo { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Control> Controls { get; set; } = new List<Control>();

    [InverseProperty("User")]
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

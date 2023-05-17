using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicaBase.Models.Entities;

public partial class Control
{
    [Key]
    [StringLength(200)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    public int PatientId { get; set; }

    [Column(TypeName = "text")]
    public string Motivo { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Tratamiento { get; set; } = null!;

    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [ForeignKey("PatientId")]
    [InverseProperty("Controls")]
    public virtual Patient Patient { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Controls")]
    public virtual User User { get; set; } = null!;
}

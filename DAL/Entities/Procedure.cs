using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text;

namespace DAL.Entities
{
    [Table("procedures", Schema = "public")]
    public class Procedure
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("duration_minutes")]
        public int DurationMinutes { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("category")]
        public string Category { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Master> Masters { get; set; }

    }
}
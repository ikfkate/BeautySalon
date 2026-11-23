using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("masters", Schema = "public")]
    public class Master
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("full_name")]
        public string FullName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("photo_url")]
        public string PhotoUrl { get; set; }

        [Column("category")]
        public string Category { get; set; }

        // Навигационные свойства для связанных таблиц
        public virtual ICollection<Appointment> Appointments { get; set; }
        //public virtual ICollection<MasterSchedule> MasterSchedules { get; set; }
        public virtual ICollection<Procedure> Procedures { get; set; }
    }
}

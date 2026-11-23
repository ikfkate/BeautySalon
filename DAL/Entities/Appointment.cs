using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("appointments", Schema = "public")]
    public class Appointment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("master_id")]
        public int MasterId { get; set; }

        [Column("procedure_id")]
        public int ProcedureId { get; set; }

        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }

        [Column("status")]
        public string Status { get; set; }



        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        // Навигационные свойства (EF сам свяжет запись с конкретным клиентом, мастером и процедурой)
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("MasterId")]
        public virtual Master Master { get; set; }

        [ForeignKey("ProcedureId")]
        public virtual Procedure Procedure { get; set; }
    }
}

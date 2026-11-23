using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int MasterId { get; set; }
        public int ProcedureId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }

        public decimal TotalPrice { get; set; } 

        // Дополнительные свойства для красивого отображения в таблице:
        public string ClientName { get; set; }
        public string MasterName { get; set; }
        public string ProcedureTitle { get; set; }
        public string MasterCategory { get; set; }

        public AppointmentDTO() { }

        public AppointmentDTO(DAL.Entities.Appointment a)
        {
            Id = a.Id;
            ClientId = a.ClientId;
            MasterId = a.MasterId;
            ProcedureId = a.ProcedureId;
            AppointmentDate = a.AppointmentDate;
            Status = a.Status;
            TotalPrice = a.TotalPrice;

            // Заполняем текстовые поля из связанных сущностей
            ClientName = a.Client.FullName;
            MasterName = a.Master.FullName;
            ProcedureTitle = a.Procedure.Title;
            MasterCategory = a.Master.Category;
        }
    }
}
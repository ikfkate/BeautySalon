using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using BLL.DTO;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class AppointmentService
    {
        public List<AppointmentDTO> GetAllAppointments()
        {
            using (var context = new BeautySalonContext())
            {
                return context.Appointments
                    .Include(a => a.Client)     // Обращение к таблице клиентов (получаем данные из БД)
                    .Include(a => a.Master)     // Мастеров
                    .Include(a => a.Procedure)  // Процедур
                    .ToList()
                    .Select(a => new AppointmentDTO(a)) // Превращение сущностей в DTO
                    .ToList();
            }
        }

    }
}
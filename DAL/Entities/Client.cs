using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("clients", Schema = "public")]
    public class Client
    {
        [Key]  // Первичный ключ и сразу автоинкрементный
        [Column("id")] // Указывает точное имя столбца в таблице БД
        public int Id { get; set; }

        [Required] // NOT NULL
        [Column("full_name")]
        public string FullName { get; set; }

        [Required]
        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        // Навигационное свойство: список всех записей клиента
        public virtual ICollection<Appointment> Appointments { get; set; } // Ленивая загрузка, данные подгрузятся из БД, когда обратимся к свойсту записей у клиента
    }
}

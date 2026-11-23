using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class ProcedureDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        // Конструктор по умолчанию
        public ProcedureDTO() { }

        // Конструктор, который превращает сущность из БД в DTO
        public ProcedureDTO(Procedure p)
        {
            Id = p.Id;
            Title = p.Title;
            Description = p.Description;
            DurationMinutes = p.DurationMinutes;
            Price = p.Price;
            Category = p.Category;
        }
    }
}

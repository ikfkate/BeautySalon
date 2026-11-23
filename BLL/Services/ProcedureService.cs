using BLL.DTO;
using DAL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class ProcedureService
    {
        // Метод для получения всех процедур
        public List<ProcedureDTO> GetAllProcedures()
        {
            using (var context = new BeautySalonContext())
            {
                // Достаем из БД и сразу конвертируем в DTO
                return context.Procedures.ToList().Select(p => new ProcedureDTO(p)).ToList();
            }
        }

        // Метод для получения одной процедуры по Id
        public ProcedureDTO GetProcedure(int id)
        {
            using (var context = new BeautySalonContext())
            {
                var p = context.Procedures.Find(id);
                if (p == null) return null;
                return new ProcedureDTO(p);
            }
        }

        // Добавление
        //public void CreateProcedure(ProcedureDTO p)
        //{
        //    using (var context = new BeautySalonContext())
        //    {
        //        var newProc = new Procedure
        //        {
        //            Title = p.Title,
        //            Description = p.Description,
        //            DurationMinutes = p.DurationMinutes,
        //            Price = p.Price,
        //            Category = p.Category
        //        };
        //        context.Procedures.Add(newProc);
        //        context.SaveChanges();
        //    }
        //}

        //// Обновление
        //public void UpdateProcedure(ProcedureDTO p)
        //{
        //    using (var context = new BeautySalonContext())
        //    {
        //        var proc = context.Procedures.Find(p.Id);
        //        if (proc != null)
        //        {
        //            proc.Title = p.Title;
        //            proc.Description = p.Description;
        //            proc.DurationMinutes = p.DurationMinutes;
        //            proc.Price = p.Price;
        //            proc.Category = p.Category;
        //            context.SaveChanges();
        //        }
        //    }
        //}
        // Добавление с проверками!!!!!!!!!!!!!!
        public void CreateProcedure(ProcedureDTO p)
        {
            // Бизнес-логика: проверяем данные перед отправкой в БД
            if (string.IsNullOrWhiteSpace(p.Title))
                throw new Exception("Название процедуры не может быть пустым.");
            if (p.Price <= 0)
                throw new Exception("Цена должна быть больше нуля.");
            if (p.DurationMinutes <= 0)
                throw new Exception("Длительность должна быть больше нуля.");

            using (var context = new BeautySalonContext())
            {
                var newProc = new Procedure
                {
                    Title = p.Title,
                    Description = p.Description,
                    DurationMinutes = p.DurationMinutes,
                    Price = p.Price,
                    Category = p.Category
                };
                context.Procedures.Add(newProc);
                context.SaveChanges();
            }
        }

        // Обновление
        public void UpdateProcedure(ProcedureDTO p)
        {
            // Бизнес-логика
            if (string.IsNullOrWhiteSpace(p.Title))
                throw new Exception("Название процедуры не может быть пустым.");
            if (p.Price <= 0)
                throw new Exception("Цена должна быть больше нуля.");

            using (var context = new BeautySalonContext())
            {
                var proc = context.Procedures.Find(p.Id);
                if (proc != null)
                {
                    proc.Title = p.Title;
                    proc.Description = p.Description;
                    proc.DurationMinutes = p.DurationMinutes;
                    proc.Price = p.Price;
                    proc.Category = p.Category;
                    context.SaveChanges();
                }
            }
        }

        // Удаление
        public void DeleteProcedure(int id)
        {
            using (var context = new BeautySalonContext())
            {
                var p = context.Procedures.Find(id);
                if (p != null)
                {
                    context.Procedures.Remove(p);
                    context.SaveChanges();
                }
            }
        }

        // Получение уникальных категорий (для выпадающего списка в интерфейсе)
        public List<string> GetAvailableCategories()
        {
            using (var context = new BeautySalonContext())
            {
                return context.Procedures
                    .Select(p => p.Category)
                    .Distinct()
                    .ToList();
            }
        }
    }
}

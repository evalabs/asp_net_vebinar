using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Vebinar.Test.Models.DTO
{
    // DTO - Data Transfer Objects - объекты, на основе которых делаются представления (Views)

    public class Auto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }


    // Ctr + Shift + B - скомпилировать всё приложение
    // F5 - debug
    // Ctrl + F5 - запуск без debug
}
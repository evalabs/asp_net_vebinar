using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Web.Vebinar.Test.Models.DTO;
using Newtonsoft.Json;

namespace Web.Vebinar.Test.Models.Repository
{
    // В этом классе мы инкапсилируем логику работы с моделью, т.е. с Auto
    public class AutoRepository
    {
        private string _pathToJson;

        // Это конструктор класса, в C# констрктор является методом, чьё имя совпадает с именем класса, в данном случае AutoRepository
        // Конструктор не имеет возвращаемого значения
        public AutoRepository(string pathToJson)
        {
            _pathToJson = pathToJson;

            // Если файл с автомобилями не найден, мы его создаём и записываем туда две квадратные скобки что для JSON означает пустой массив.
            // То есть, записываем в этот файл пустой JSON-массив.

            if (!File.Exists(_pathToJson))
            {
                using (var sw = File.CreateText(_pathToJson))
                {
                    sw.WriteLine("[]");
                }
            }
        }

        // Вспомогательный метод. Читает файл с JSON-ом, десериализует его (превращает в объекты) и отдаёт результат с помощью return
        private List<Auto> _getAutoList()
        {
            using (var sr = new StreamReader(_pathToJson))
            {
                var json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Auto>>(json);
            }
        }

        // Тоже вспомогательный метод. Делает обратную операцию по отнощению к _getAutoList(). Получает на входе объекты, сериализует их (превращает в JSON) и записывает в файл
        private void _setAutoList(List<Auto> autoList)
        {
            using (var sw = new StreamWriter(_pathToJson, false))
            {
                var json = JsonConvert.SerializeObject(autoList);
                sw.Write(json);
            }
        }

        // Возвращает список автомобилей, если нет ни одного автомобиля возвращает пустой список
        public List<Auto> GetAutoList()
        {
            var autoList = _getAutoList();
            return autoList ?? new List<Auto>(); // Эта хитрая конструкция означает, что если autoList = null, то верни нам new List<Auto>(), т.е. пустой список
        }

        // Возвращает конкретный автомобиль по его id
        public Auto GetAuto(Guid id)
        {
            var autoList = _getAutoList();
            return autoList.Where(a => a.Id == id).SingleOrDefault();
        }

        // Сохраняет автомобиль, если у него нет Id, то создаёт новый
        public Auto SaveAuto(Auto auto)
        {
            var autoList = GetAutoList();

            if (auto.Id == null)
            {
                auto.Id = Guid.NewGuid();
                autoList.Add(auto);
            }
            else
            {
                var dbAuto = autoList.Where(a => a.Id == auto.Id).SingleOrDefault();
                dbAuto.Name = auto.Name;
                dbAuto.Year = auto.Year;
                dbAuto.Price = auto.Price;
            }

            _setAutoList(autoList);

            return auto;
        }

        // Удаляет автомобиль
        public void DeleteAuto(Guid id)
        {
            var autoList = GetAutoList();
            var dbAuto = autoList.Where(a => a.Id == id).SingleOrDefault();
            autoList.Remove(dbAuto);
            _setAutoList(autoList);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Alym.Shared.Models;

namespace Alym.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresetsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<BusinessPreset> Get()
        {
            return new List<BusinessPreset>
            {
                new BusinessPreset
                {
                    Name = "Кофейня",
                    Questions = new List<Question>
                    {
                        new Question { Text = "Сколько чашек в день?" },
                        new Question { Text = "Средний чек (руб)?" },
                        new Question { Text = "Себестоимость напитка (руб)?" },
                        new Question { Text = "Аренда в месяц (руб)?" }
                    }
                },
                new BusinessPreset
                {
                    Name = "Компьютерный клуб",
                    Questions = new List<Question>
                    {
                        new Question { Text = "Сколько компьютеров?" },
                        new Question { Text = "Часы работы в день (в среднем)?" },
                        new Question { Text = "Средняя цена за час (руб)?" },
                        new Question { Text = "Эл.энергия на 1 ПК/час (кВт·ч)?" },
                        new Question { Text = "Зарплата администратора в месяц (руб)?" }
                    }
                },
                new BusinessPreset
                {
                    Name = "Парикмахерская",
                    Questions = new List<Question>
                    {
                        new Question { Text = "Сколько мастеров?" },
                        new Question { Text = "Среднее количество клиентов в день на мастера?" },
                        new Question { Text = "Средний чек (руб)?" },
                        new Question { Text = "Расходы на материалы в месяц (руб)?" }
                    }
                }
            };
        }
    }
}

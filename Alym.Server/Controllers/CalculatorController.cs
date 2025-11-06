using Microsoft.AspNetCore.Mvc;

namespace Alym.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        // -------------------------------
        // 1️⃣ Расчёт электроэнергии
        // -------------------------------
        [HttpGet("electricity")]
        public IActionResult CalculateElectricity(decimal consumptionKwh, decimal tariffPerKwh)
        {
            if (consumptionKwh <= 0 || tariffPerKwh <= 0)
                return BadRequest("Введите корректные значения.");

            var total = consumptionKwh * tariffPerKwh;
            return Ok(new
            {
                Type = "Электроэнергия",
                Consumption = consumptionKwh,
                Tariff = tariffPerKwh,
                Total = total,
                Message = $"Итого к оплате: {total:0.00} руб."
            });
        }

        // -------------------------------
        // 2️⃣ Расчёт воды
        // -------------------------------
        [HttpGet("water")]
        public IActionResult CalculateWater(decimal volumeM3, decimal tariffPerM3)
        {
            if (volumeM3 <= 0 || tariffPerM3 <= 0)
                return BadRequest("Введите корректные значения.");

            var total = volumeM3 * tariffPerM3;
            return Ok(new
            {
                Type = "Вода",
                Volume = volumeM3,
                Tariff = tariffPerM3,
                Total = total,
                Message = $"Стоимость воды: {total:0.00} руб."
            });
        }

        // -------------------------------
        // 3️⃣ Расчёт рентабельности бизнеса
        // -------------------------------
        [HttpGet("profitability")]
        public IActionResult CalculateProfitability(decimal revenue, decimal expenses)
        {
            if (revenue <= 0)
                return BadRequest("Доход должен быть больше нуля.");

            var profit = revenue - expenses;
            var profitability = (profit / revenue) * 100;

            return Ok(new
            {
                Type = "Рентабельность бизнеса",
                Revenue = revenue,
                Expenses = expenses,
                Profit = profit,
                Profitability = $"{profitability:0.00}%",
                Message = $"Рентабельность бизнеса: {profitability:0.00}% (прибыль: {profit:0.00} руб.)"
            });
        }

        // -------------------------------
        // 4️⃣ Расчёт точки безубыточности
        // -------------------------------
        [HttpGet("breakeven")]
        public IActionResult CalculateBreakEven(decimal fixedCosts, decimal pricePerUnit, decimal variableCostPerUnit)
        {
            if (pricePerUnit <= variableCostPerUnit)
                return BadRequest("Цена за единицу должна быть больше переменных затрат.");

            var breakevenPoint = fixedCosts / (pricePerUnit - variableCostPerUnit);
            return Ok(new
            {
                Type = "Точка безубыточности",
                FixedCosts = fixedCosts,
                PricePerUnit = pricePerUnit,
                VariableCostPerUnit = variableCostPerUnit,
                UnitsToBreakEven = breakevenPoint,
                Message = $"Необходимо продать {breakevenPoint:0.00} единиц для выхода в ноль."
            });
        }
    }
}

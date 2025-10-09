namespace Alym.Shared.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Region { get; set; } = string.Empty;
        public decimal Price { get; set; } // руб/кВт·ч
    }
}

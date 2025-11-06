namespace Alym.Shared.Models
{
    public class TariffCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Электричество", "Вода", "Газ"
    }

    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // "Moscow", "Kazan" и т.д.
        public string? Note { get; set; } // необязательная заметка
    }

    public class Tariff
    {
        public int Id { get; set; }
        public int TariffCategoryId { get; set; }
        public TariffCategory? TariffCategory { get; set; }

        public int RegionId { get; set; }
        public Region? Region { get; set; }

        public decimal PricePerUnit { get; set; } // например руб./кВт·ч или руб./м3
        public string Unit { get; set; } = "unit"; // "руб/кВт·ч", "руб/м³" и т.д.
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}

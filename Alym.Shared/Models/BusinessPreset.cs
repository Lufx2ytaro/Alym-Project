namespace Alym.Shared.Models
{
    public class BusinessPreset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
    }

    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public decimal Value { get; set; } = 0m;
    }
}
namespace SanayiCebimdeBackend.Domain.Entities
{
    public class Yorum
    {
        public int Id { get; set; }
        public int UstaId { get; set; }
        public Ustalar Usta { get; set; }
        public string User { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
    }
}

namespace SanayiCebimdeBackend.Domain.Entities
{
    public class Galeri
    {
        public int Id { get; set; }
        public int UstaId { get; set; }
        public Ustalar Usta { get; set; }
        public string Name { get; set; }
    }
}

namespace SanayiCebimdeBackend.Domain.Entities
{
    public class Usta
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Location { get; set; }
        public double Rating { get; set; }
        public int Reviews { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int MemberSince { get; set; }
        public int TotalJobs { get; set; }
        public int SatisfactionRate { get; set; }

        public ICollection<Yorum> Yorumlar { get; set; }
        public ICollection<Galeri> Galeri { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }
}

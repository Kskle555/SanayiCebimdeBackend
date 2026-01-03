using System.ComponentModel.DataAnnotations.Schema;

namespace SanayiCebimdeBackend.Domain.Entities
{
    [Table("Ustalar", Schema = "sanayice_35")]
    public class Ustalar
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Profession { get; set; }
        public string? Location { get; set; }
        public decimal? Rating { get; set; }
        public int? Reviews { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? MemberSince { get; set; }
        public int? TotalJobs { get; set; }
        public int? SatisfactionRate { get; set; }
        public string? Phone { get; set; }
        public DateTime? Date { get; set; }
        public string? ImageURL { get; set; }

        public ICollection<Yorum> Yorumlar { get; set; } = new List<Yorum>();
        public ICollection<Galeri> Galeri { get; set; } = new List<Galeri>();
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();

    }
}

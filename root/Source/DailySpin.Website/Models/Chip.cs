using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class Chip
    {
        [Key]
        public int Id { get; set; }
        public string Image { get; set; }
    }
}

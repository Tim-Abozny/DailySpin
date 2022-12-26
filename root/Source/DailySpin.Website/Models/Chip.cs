using System.ComponentModel.DataAnnotations;
using DailySpin.Website.Enums;

namespace DailySpin.Website.Models
{
    public class Chip
    {
        [Key]
        public Guid Id { get; set; }
        public string Image { get; set; }
        public ChipColor ColorType { get; set; }
        public bool WinChip { get; set; }
    }
}

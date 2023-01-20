using DailySpin.Website.Enums;
using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class Chip
    {
        [Key]
        public Guid Id { get; set; }
        public byte[] Image { get; set; }
        public ChipColor ColorType { get; set; }
        public uint WinChip { get; set; }
    }
}

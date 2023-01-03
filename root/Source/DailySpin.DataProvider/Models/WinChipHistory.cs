using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class WinChipHistory
    {
        [Key]
        public int Id { get; set; }
        public List<Chip> WinChips { get; set; }
    }
}

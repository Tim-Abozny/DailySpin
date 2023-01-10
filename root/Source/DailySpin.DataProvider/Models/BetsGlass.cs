using DailySpin.Website.Enums;
using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class BetsGlass
    {
        [Key]
        public Guid Id { get; set; }
        public byte[]? GlassImage { get; set; }
        public ushort BetMultiply { get; set; }
        public uint BetsCount { get; set; }
        public ulong TotalBetSum { get; set; }
        public List <Bet> Bets { get; set; }
        public ChipColor ColorType { get; set; }
    }
}

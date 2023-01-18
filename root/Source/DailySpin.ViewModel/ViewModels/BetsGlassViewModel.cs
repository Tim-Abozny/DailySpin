using DailySpin.Website.Enums;
using DailySpin.Website.Models;

namespace DailySpin.ViewModel.ViewModels
{
    public class BetsGlassViewModel
    {
        public ChipColor ColorType { get; set; }
        public byte[] GlassImage { get; set; }
        public ushort BetMultiply { get; set; }
        public List<Bet> Bets { get; set; }
        public ulong TotalBetSum { get; set; }
    }
}

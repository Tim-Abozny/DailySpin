namespace DailySpin.Website.Models
{
    public class BetsGlass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int BetMultiply { get; set; }
        public int BetsCount { get; set; }
        public int TotalBetSum { get; set; }
        public List <UserAccount> Bets { get; set; } // is it correct... ?
    }
}

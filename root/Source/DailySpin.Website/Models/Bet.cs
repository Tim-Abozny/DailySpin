using System.ComponentModel.DataAnnotations;

namespace DailySpin.Website.Models
{
    public class Bet
    {
        [Key]
        public Guid Id { get; set; }
        public byte[] UserImage { get; set; }
        public string UserName { get; set; }
        public Guid UserID { get; set; }
        public uint UserBet { get; set; }
    }
}

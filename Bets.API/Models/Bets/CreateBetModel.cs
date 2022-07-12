using System.ComponentModel.DataAnnotations;

namespace Bets.API.Models.Bets
{
    public class CreateBetRequest : BetsRequestBase
    {
        [Required]
        public long SelectionId { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Stake { get; set; }
    }
}

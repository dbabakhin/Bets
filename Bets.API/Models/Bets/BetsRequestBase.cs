using System.ComponentModel.DataAnnotations;

namespace Bets.API.Models.Bets
{
    public abstract class BetsRequestBase
    {
        [Required]
        public string UserToken { get; set; }
    }
}

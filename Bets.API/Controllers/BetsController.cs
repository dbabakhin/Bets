using Bets.API.App;
using Bets.API.Models.Bets;
using Microsoft.AspNetCore.Mvc;

namespace Bets.API.Controllers
{
    [ApiController]
    [Route("Bets")]
    public class BetsController : ControllerBase
    {
        private readonly BetsProcessor _betsProcessor;

        public BetsController(BetsProcessor betsApplicationService)
        {
            _betsProcessor = betsApplicationService ?? throw new ArgumentNullException(nameof(betsApplicationService));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] CreateBetRequest request, CancellationToken ct)
        {
            var b = await _betsProcessor.CreateBetAsync(request.UserToken, request.SelectionId, request.Stake, ct);
            return CreatedAtAction(nameof(CreateBet), b.BetId);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyBets([FromQuery] GetMyBetsRequest request, CancellationToken ct)
        {
            var res = await _betsProcessor.GetUserBetsAsync(request.UserToken, ct);
            return Ok(res);
        }
    }
}

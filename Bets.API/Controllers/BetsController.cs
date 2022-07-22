using Bets.API.App;
using Bets.API.Models.Bets;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bets.API.Controllers
{
    [ApiController]
    [Route("Bets")]
    public class BetsController : ControllerBase
    {
        private readonly BetsProcessor _betsProcessor;
        private readonly ILogger<BetsController> _logger;

        public BetsController(BetsProcessor betsApplicationService, ILogger<BetsController> logger)
        {
            _betsProcessor = betsApplicationService ?? throw new ArgumentNullException(nameof(betsApplicationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBet([FromBody] CreateBetRequest request, CancellationToken ct)
        {
            using var s = _logger.BeginScope("selectionId: {selection}, stake: {stake}, token: {token}", request.SelectionId, request.Stake, request.UserToken);
            var b = await _betsProcessor.CreateBetAsync(request.UserToken, request.GetModel(), ct);
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

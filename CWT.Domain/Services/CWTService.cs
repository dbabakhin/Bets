using CWT.Domain.Interfaces;
using Shared.Common.Interfaces;
using Shared.Common.Messages;

namespace CWT.Domain.Services
{
    public class CWTService : ICWTService
    {
        private readonly ICWTRepository _cwtRepository;
        private readonly IMessageProducer<string, BetConfirmResultMessage> _producer;

        public CWTService(ICWTRepository cwtRepository, IMessageProducer<string, BetConfirmResultMessage> producer)
        {
            _cwtRepository = cwtRepository ?? throw new ArgumentNullException(nameof(cwtRepository));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public async Task<BetConfirmResultMessage> CheckEnoughtMoneyAsync(BetConfirmRequestMessage request, CancellationToken ct)
        {
            var userBalance = await _cwtRepository.GetUserBalanceAsync(request.UserId);
            var msg = new BetConfirmResultMessage(request.BetId, request.SelectionId, request.Stake, request.UserId, request.Stake <= userBalance);
            await _producer.ProduceAsync(null, msg, ct);
            return msg;
        }
    }
}

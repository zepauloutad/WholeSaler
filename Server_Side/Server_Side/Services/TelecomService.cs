using Grpc.Core;
using System.Threading.Tasks;

namespace Server_Side.Services
{
    public class TelecomService : Telecom.TelecomBase
    {
        private readonly ReserveService _reserveService;
        private readonly ActivateService _activateService;
        private readonly DeactivateService _deactivateService;
        private readonly TerminateService _terminateService;

        public TelecomService(
            ReserveService reserveService,
            ActivateService activateService,
            DeactivateService deactivateService,
            TerminateService terminateService)
        {
            _reserveService = reserveService;
            _activateService = activateService;
            _deactivateService = deactivateService;
            _terminateService = terminateService;
        }

        public override Task<ReserveReply> Reserve(ReserveRequest request, ServerCallContext context)
        {
            var result = _reserveService.Reserve(request.Domicile, request.Mode);
            return Task.FromResult(new ReserveReply { AdminNumber = result.AdminNumber, ReservationStatus = result.ReservationStatus });
        }

        public override Task<ActivateReply> Activate(ActivateRequest request, ServerCallContext context)
        {
            var result = _activateService.Activate(request.AdminNumber);
            return Task.FromResult(new ActivateReply { EstimatedTime = result.EstimatedTime, ActivationStatus = result.ActivationStatus });
        }

        public override Task<DeactivateReply> Deactivate(DeactivateRequest request, ServerCallContext context)
        {
            var result = _deactivateService.Deactivate(request.AdminNumber);
            return Task.FromResult(new DeactivateReply { EstimatedTime = result.EstimatedTime, DeactivationStatus = result.DeactivationStatus });
        }

        public override Task<TerminateReply> Terminate(TerminateRequest request, ServerCallContext context)
        {
            var result = _terminateService.Terminate(request.AdminNumber);
            return Task.FromResult(new TerminateReply { EstimatedTime = result.EstimatedTime, TerminationStatus = result.TerminationStatus });
        }
    }
}

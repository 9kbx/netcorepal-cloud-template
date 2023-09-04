﻿using ABC.Template.Web.Application.Commands;
using DotNetCore.CAP;
using MediatR;
using NetCorePal.Extensions.DistributedTransactions;
using NetCorePal.Extensions.Primitives;

namespace ABC.Template.Web.Application.IntegrationEventHandlers
{
    public class OrderPaidIntegrationEventHandler : IIntegrationEventHandler<OrderPaidIntegrationEvent>
    {
        readonly ILogger _logger;
        readonly IMediator _mediator;
        public OrderPaidIntegrationEventHandler(IMediator mediator, ILogger<OrderPaidIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public Task HandleAsync(OrderPaidIntegrationEvent eventData, CancellationToken cancellationToken = default)
        {
            var cmd = new OrderPaidCommand(eventData.OrderId);
            return _mediator.Send(cmd);
        }

    }
}

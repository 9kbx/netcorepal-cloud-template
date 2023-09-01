﻿using ABC.Template.Domain.AggregatesModel.OrderAggregate;
using ABC.Template.Infrastructure.Repositories;
using ABC.Template.Web.Application.IntegrationEventHandlers;
using NetCorePal.Extensions.Mappers;
using NetCorePal.Extensions.Primitives;
using NetCorePal.Extensions.Repository;

namespace ABC.Template.Web.Application.Commands
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderId>
    {
        readonly IOrderRepository _orderRepository;
        readonly ILogger _logger;
        readonly IMapperProvider _mapperProvider;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapperProvider mapperProvider, ILogger<OrderPaidIntegrationEventHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _mapperProvider = mapperProvider;
        }




        public async Task<OrderId> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = request.MapTo<Order>(_mapperProvider);
            order = await _orderRepository.AddAsync(order, cancellationToken);
            _logger.LogInformation($"order created, id:{order.Id}");
            return order.Id;
        }
    }
}

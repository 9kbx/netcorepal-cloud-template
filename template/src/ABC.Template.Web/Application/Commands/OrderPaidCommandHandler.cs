﻿using ABC.Template.Infrastructure.Repositories;
using NetCorePal.Extensions.Primitives;

namespace ABC.Template.Web.Application.Commands
{
    public class OrderPaidCommandHandler : ICommandHandler<OrderPaidCommand>
    {
        readonly IOrderRepository _orderRepository;
        public OrderPaidCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task Handle(OrderPaidCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order == null)
            {
                throw new KnownException($"未找到订单，OrderId = {request.OrderId}");
            }
            order.OrderPaid();
        }
    }
}

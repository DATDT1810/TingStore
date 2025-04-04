﻿using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands;
using Order.Application.DTOs;
using Order.Application.Handlers;
using Order.Application.Queries;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IMediator mediator, ILogger<OrderController> logger , IPublishEndpoint publishEndpoint, IHttpClientFactory httpClientFactory)
        {
            _mediator = mediator;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _mediator.Send(new GetOrdersQuerry());
            return Ok(result);
        }

        [HttpGet]
        [Route("GetOrderByCustomerId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByCustomerId([FromRoute] int id)
        {
            var request = new GetOrderByCustomerIdQuerry(id);
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetOrderByOrderId/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByOrderId([FromRoute] string id)
        {
            var request = new GetOrderByOrderIdQuerry(id);
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand updateOrderCommand)
        {
            var result = await _mediator.Send(updateOrderCommand);
            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteOrderItems")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOrderItems([FromBody] DeleteOderItemCommand deleteOderItemCommand)
        {
            var result = await _mediator.Send(deleteOderItemCommand);
            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderCommand deleteOrderCommand)
        {
            var result = await _mediator.Send(deleteOrderCommand);
            return Ok(result);
        }

        [HttpPost]
        [Route("PayOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PayOrder([FromBody] Guid OrderId )
        {
            var command = new PayOrderCommand
            {
                OrderId = OrderId
            };
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest();
            }
           
            // gọi đối tượng trên service payment
            var request = _httpClientFactory.CreateClient();
            // cổng http của payment
            var response = await request.PostAsJsonAsync("http://localhost:5007/api/Payment", new
            {
                OrderID = OrderId,
                Amount = result.Amount
            });

            if(!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var paymentUrl = await response.Content.ReadAsStringAsync();
            return Ok(new
            {
                PaymentUrl = paymentUrl
            });
        }

    }
}

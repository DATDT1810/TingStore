﻿using System.IO.Pipes;
using System.Net;
using System.Threading.Tasks;
using Cart.Application.Commands;
using Cart.Application.Dtos;
using Cart.Application.Mappers;
using Cart.Application.Queries;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        public CartController(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CartShoppingDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCart(int id)
        {
            var query = new GetCartByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateCart")]
        [ProducesResponseType(typeof(CartShoppingDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCart([FromBody] CartShoppingDTO cartShoppingDTO)
        {
            if (cartShoppingDTO == null)
            {
                return BadRequest();
            }
            var command = new CreateCartCommand(cartShoppingDTO);
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var command = new DeleteCartCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [Route("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckoutCart([FromBody] CartCheckoutCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest();
            }
            var msg = CartMapper.Mapper.Map<CartCheckoutEvent>(result);
            await _publishEndpoint.Publish(msg);

            var ListOfProductIds = result.Items.ConvertAll(x => x.ProductId);
            var deleteCommand = new DeleteProductFromCartCommand(result.Id, ListOfProductIds);
            await _mediator.Send(deleteCommand);


            return Accepted();
        }

        [HttpPost]
        [Route("DeleteProductFromCartCommand")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult DeleteItems([FromBody] DeleteProductFromCartCommand command)
        {
            var result = _mediator.Send(command);
            return Ok(result);
        }
    }
}


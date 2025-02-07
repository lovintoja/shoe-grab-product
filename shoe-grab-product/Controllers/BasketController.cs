using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeGrabCommonModels;
using ShoeGrabProductManagement.Dto;
using System.Security.Claims;


namespace ShoeGrabProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(IBasketService basketService, IMapper mapper, ILogger<BasketController> logger)
        {
            _basketService = basketService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var userId = GetUserId();

            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var basket = await _basketService.GetBasket(userId.Value);
            if (basket != null)
            {
                var mappedBasket = _mapper.Map<BasketDto>(basket);
                return Ok(mappedBasket);
            }

            basket = await _basketService.CreateBasket(userId.Value);

            if (basket != null)
            {
                var mappedBasket = _mapper.Map<BasketDto>(basket);
                return Ok(basket);
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveBasket()
        {
            var userId = GetUserId();

            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var success = await _basketService.RemoveBasket(userId.Value);
            if (!success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateBasket(UpdateBasketDto basket)
        {
            var userId = GetUserId();

            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var mappedBasket = new Basket
            {
                Items = basket.Items.Select(x => new BasketItem
                {
                    ProductId = x.Id,
                    Quantity = x.Quantity
                }).ToList()
            };

            var success = await _basketService.UpdateBasket(userId.Value, mappedBasket);
            if (!success)
            {
                return BadRequest();
            }
            return Ok();
        }
        
        private int? GetUserId()
        {
            if (User.Identity?.IsAuthenticated is not null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.Authentication);
                if (userIdClaim != null)
                {
                    return int.Parse(userIdClaim.Value);
                }
            }
            return null;
        }
    }
}

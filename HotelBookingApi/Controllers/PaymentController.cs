using HotelBookingApi.Dtos;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            _config = config;
            // لو ما ضبطته في Program.cs
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }


        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentRequest request)
        {
            if (request?.Amount <= 0) return BadRequest("Amount must be > 0");

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp", // أو usd حسب اللي عايز تجربه
                        UnitAmount = request.Amount, // قيمة بـ cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.Description ?? request.RoomType ?? "Room Booking"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/success",
                CancelUrl = "http://localhost:4200/payment"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return Ok(new { id = session.Id });
        }
    }
}

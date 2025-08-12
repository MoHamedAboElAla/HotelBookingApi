using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace HotelBookingApi.Controllers
{
    /*
    [Route("api/[controller]")]
    [ApiController]
    public class StripesController : ControllerBase
    {
        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] PaymentRequest request)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = request.Amount * 100, // بالمليم
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = request.ProductName
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:4200/success",
                CancelUrl = "http://localhost:4200/cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Ok(new { id = session.Id });
        }
    }
    */
    /*
    public class PaymentRequest
    {
        public string ProductName { get; set; }
        public long Amount { get; set; }
    }
    */
}

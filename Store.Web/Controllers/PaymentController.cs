using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;

namespace Store.Web.Controllers
{
  
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        const string endpointSecret = "whsec_ecfe9571b0b554ea2c820d3e57a33c0e179c9ad279ac296b3a7c53d673bfa127";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(CustomerBasketDto input)
        => Ok(await _paymentService.CreateOrUpdatePaymentIntent(input));

        [HttpPost]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent paymentIntent;

                // Handle the event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Failed : ", paymentIntent.Id);
                    var order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Failed : ", order.Id);
                    return Ok();
                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment Succeeded : ", paymentIntent.Id);
                    var order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Succeeded : ", order.Id);
                    return Ok();
                }
                else if (stripeEvent.Type == "payment_intent.created")
                {
                    _logger.LogInformation("Payment Intent Created");
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}

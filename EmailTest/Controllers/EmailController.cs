using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailTest.Classes;
using Microsoft.AspNetCore.Http;

namespace EmailTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        [Route("/trip-approval")]
        public IActionResult TripApproval(TravelRequest travelRequest, EmailType type)
        {
            try
            {
                if (travelRequest == null)
                {
                    return BadRequest();
                }

                var email = new EmailService();
                email.CreateAndSendTripApprovalEmail(travelRequest, type);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        [Route("/reject-travel-request")]
        public void RejectTravelRequest()
        {
            var rejected = "rejected";
        }

        [HttpGet]
        [Route("/approve-travel-request")]
        public void ApproveTravelRequest()

        {
            var approved = "approved";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBooking.Models;
using ShowBooking.Service;

namespace ShowBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreBookingsController : ControllerBase
    {
        private readonly ShowContext _context;

        public TheatreBookingsController(ShowContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TheatreBooking>>> GetTheatreBookings()
        {
            return await _context.TheatreBookings.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TheatreBooking>> GetTheatreBooking(int id)
        {
            var theatreBooking = await _context.TheatreBookings.FindAsync(id);

            if (theatreBooking == null)
            {
                return NotFound();
            }

            return theatreBooking;
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<object>>> UserTheatreBooking(int id)
        {
         
            var theatreBookings = await _context.TheatreBookings
                .Where(tb => tb.UserID == id)
                .Select(tb => new
                {
                    TheatreName = tb.TheatreShow.TheatreScreen.Theatre.TheatreName,
                    MovieName = tb.TheatreShow.Movie.MovieName,
                    ShowDate = tb.TheatreShow.ShowDate,
                    ShowTime = tb.TheatreShow.ShowTime,
                    SeatNumbers = tb.TheatreTickets.Select(t => t.ShowSeat.SeatNumber).ToList(),
                    TotalPrice = tb.TheatreTickets.Sum(t => t.Price)
                })
                .ToListAsync();

            if (theatreBookings == null || !theatreBookings.Any())
            {
                return NotFound(new { Message = "No bookings found for this user." });
            }

            return Ok(theatreBookings);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostTheatreBooking([FromBody] TheatreBookingRequestDTO bookingRequest)
        {
            
            var theatreBooking = new TheatreBooking
            {
                UserID = bookingRequest.UserId,
                ShowID = bookingRequest.ShowId,
                TotalAmount = bookingRequest.TotalAmount,
                Status = "Confirmed", 
                BookingDate = DateTime.Now 
            };

            User user = _context.Users.Find(bookingRequest.UserId);

            ICollection<ShowSeat> tempList = new List<ShowSeat>();

            _context.TheatreBookings.Add(theatreBooking);
            await _context.SaveChangesAsync(); 

            foreach (var seatId in bookingRequest.SeatIds)
            {
                var showSeat = await _context.ShowSeats.Include(s => s.TheatreShow).ThenInclude(m => m.Movie).FirstOrDefaultAsync(s => s.ShowSeatID == seatId);
                var tickettype = _context.TicketTypes.FirstOrDefault(t => t.TicketTypeName == showSeat!.TicketTypeName);
                if (showSeat != null)
                {
                    tempList.Add(showSeat);
                    var theatreTicket = new TheatreTicket
                    {
                        BookingID = theatreBooking.BookingID,
                        ShowSeatID = showSeat.ShowSeatID,
                        TicketTypeID = tickettype!.TicketTypeID,
                        Price = showSeat.TicketPrice, 
                        Status = "Booked" 
                    };

                    _context.TheatreTickets.Add(theatreTicket);

                    showSeat.IsBooked = true; 
                }
            }

            await _context.SaveChangesAsync();

            var createEventRequest = new GoogleCalender
            {
                UserId = bookingRequest.UserId,
                Summary = "Your Ticket Booking",
                Start = DateTime.UtcNow.AddHours(1),
                End = DateTime.UtcNow.AddHours(2)
            };

            var result = await CreateEvent(createEventRequest,theatreBooking, tempList.First());
            var pdfPath = GenerateBookingPdf(bookingRequest, theatreBooking, user, tempList);
            await SendBookingEmail(user.Email,pdfPath);
            return Ok(result);

        }


        private bool TheatreBookingExists(int id)
        {
            return _context.TheatreBookings.Any(e => e.BookingID == id);
        }

        private async Task<IActionResult> CreateEvent(GoogleCalender eventRequest, TheatreBooking theatreBooking, ShowSeat showSeats)
        {
            var user = await _context.Users.FindAsync(eventRequest.UserId);
            if (user == null) return NotFound("User not found.");

            var accessToken = user.GoogleAccessToken;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            TimeZoneInfo indiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            DateTime showStartInIST = TimeZoneInfo.ConvertTimeFromUtc(showSeats.TheatreShow.ShowDate.ToUniversalTime(), indiaTimeZone);
            DateTime showEndInIST = showStartInIST.AddHours(1);

            var showDateTime = showSeats.TheatreShow.ShowDate.Add(showSeats.TheatreShow.ShowTime);

            var eventPayload = new
            {
                summary = showSeats.TheatreShow.Movie.MovieName,
                start = new
                {
                    dateTime = showDateTime.ToString("yyyy-MM-ddTHH:mm:ss"), // Combine ShowDate and ShowTime
                    timeZone = "Asia/Kolkata"  // Timezone for India
                },
                end = new
                {
                    dateTime = showDateTime.AddHours(2).ToString("yyyy-MM-ddTHH:mm:ss"), // 2 hours after start time
                    timeZone = "Asia/Kolkata"  // Timezone for India
                }
            };




            // Serialize the payload to JSON
            var jsonContent = JsonSerializer.Serialize(eventPayload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Make the request to create the event
            var response = await client.PostAsync("https://www.googleapis.com/calendar/v3/calendars/primary/events", content);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                return Ok("Event created successfully.");
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return BadRequest($"Failed to create event: {response.StatusCode} - {errorResponse}");
        }

        private byte[] GenerateBookingPdf(TheatreBookingRequestDTO bookingRequest, TheatreBooking theatreBooking, User user, ICollection<ShowSeat> showLists)
        {
            using (var ms = new MemoryStream())
            {
                // Create a new PDF document
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 25, 25);
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);
                document.Open();

                // Define font styles
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, new BaseColor(0, 0, 255));  // Blue color
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, new BaseColor(0, 0, 0));  // Black color
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, new BaseColor(0, 0, 0));       // Black color

                // Add the title
                var title = new iTextSharp.text.Paragraph("Booking Confirmation\n\n", titleFont);
                title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                document.Add(title);

                // Create a table for booking details
                var bookingTable = new PdfPTable(2) { WidthPercentage = 100 };
                bookingTable.SetWidths(new float[] { 1, 2 });
                
                // Add booking details with styling
                bookingTable.AddCell(new PdfPCell(new Phrase("Booking ID:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(theatreBooking.BookingID .ToString(), normalFont)) { Border = 0 });

                bookingTable.AddCell(new PdfPCell(new Phrase("User:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase(user.FirstName, normalFont)) { Border = 0 });
                
                bookingTable.AddCell(new PdfPCell(new Phrase("Seats:", headerFont)) { Border = 0 });
                foreach (var seat in showLists)
                {   
                    bookingTable.AddCell(new PdfPCell(new Phrase(seat.SeatNumber, normalFont)) { Border = 0 });
                }

                bookingTable.AddCell(new PdfPCell(new Phrase("Total Amount:", headerFont)) { Border = 0 });
                bookingTable.AddCell(new PdfPCell(new Phrase("₹" + theatreBooking.TotalAmount.ToString("N2"), normalFont)) { Border = 0 });

                document.Add(bookingTable);

                // Add space before QR code
                document.Add(new iTextSharp.text.Paragraph("\n\n"));

                

                // Add a simple line separator using a table with a bottom border
                var separatorTable = new PdfPTable(1) { WidthPercentage = 100 };
                var separatorCell = new PdfPCell { BorderWidthBottom = 1f, BorderColorBottom = new BaseColor(128, 128, 128), Padding = 5f };
                separatorTable.AddCell(separatorCell);
                document.Add(separatorTable);

                // Add more spacing and the document close
                document.Add(new iTextSharp.text.Paragraph("\n\nThank you for booking with us!", normalFont));
                document.Add(new iTextSharp.text.Paragraph("\nBy ShowSpot Team", normalFont));
                document.Close();

                return ms.ToArray();
            }
        }

        private async Task SendBookingEmail(string userEmail, byte[] pdfPath)
        {
            string Subject = "Your Theatre Booking Confirmation";
            string Body = "Please find attached your booking details for the show.";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("showspot.booking@gmail.com", "whiw jkvl zboa xkkp"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("showspot.booking@gmail.com"),
                Subject = Subject,
                Body = Body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(userEmail);

            if (pdfPath != null)
            {
                using (var memoryStream = new MemoryStream(pdfPath))
                {
                    var pdfAttachment = new Attachment(memoryStream, "Ticket.pdf", "application/pdf");
                    mailMessage.Attachments.Add(pdfAttachment);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            else
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
        }

    }
}

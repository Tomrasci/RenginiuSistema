using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RenginiuSistema.Models;
using RenginiuSistema.ViewModels;
using RenginiuSistema.Views.Tickets;

namespace RenginiuSistema.Controllers
{
    public class TicketsController : Controller
    {
        [Route("~/Tickets/{eventId:int}")]
        [Authorize(Roles = "Participant")]
        public ViewResult OpenTickets(int eventId)
        {
            Event e = Event.GetEvent(eventId);
            List<Order> orders = Order.GetOrders();
            List<Ticket> tickets = Ticket.SelectTicketsList(orders, e);

            return View("~/Views/Tickets/TicketsView.cshtml", new TicketsViewModel
            {
                Event = e,
                Tickets = tickets
            });
        }

        [Route("~/Ticket/{ticketId:int}")]
        [Authorize(Roles = "Participant")]
        public ViewResult DesiredTicketSelect(int ticketId)
        {
            Ticket t = Ticket.SelectTicketInformation(ticketId);
            return View("~/Views/Tickets/TicketView.cshtml", t);
        }

        [Route("~/Tickets/{eventId:int}/Purchase")]
        [Authorize(Roles = "Participant")]
        public ViewResult OpenTicketPurchase(int eventId)
        {
            Event e = Event.GetEvent(eventId);
            Ticket t = Ticket.GetUnpurchasedTicket(e);

            return View("~/Views/Tickets/DataEntryView.cshtml", new DataEntryViewModel
            {
                Event = e,
                Ticket = t
            });
        }

        [HttpPost]
        [Authorize(Roles = "Participant")]
        public ActionResult SubmitPurchaseData(DataEntryViewModel model)
        {
            bool correct = ValidateEnteredData(model);

            if (correct)
            {
                Invoice i = Invoice.FormAnInvoice(model.Event.TicketPrice);
                Order o = Order.FormAnOrder(model.Ticket, i);
                bool paid = OnlineBankBoundary.SendOrderDetailsToOnlineBank();

                if (paid)
                {
                    Order.SaveOrder(o);
                    Invoice.SaveInvoice(i);

                    return View("~/Views/Tickets/DataEntryView.cshtml", new DataEntryViewModel
                    {
                        Event = model.Event,
                        Message = "Ticket purchased successfully"
                    });
                }
                else
                {
                    Order.CancelOrder(o);

                    return View("~/Views/Tickets/DataEntryView.cshtml", new DataEntryViewModel
                    {
                        Event = model.Event,
                        Error = true,
                        Message = "Purchase failed"
                    });
                }
            }
            else
            {
                return View("~/Views/Tickets/DataEntryView.cshtml", model);
            }
        }

        public bool ValidateEnteredData(DataEntryViewModel model)
        {
            return TryValidateModel(model);
        }
    }
}
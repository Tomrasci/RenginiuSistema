using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using RenginiuSistema.Models;
using RenginiuSistema.ViewModels;
using Rotativa;

namespace RenginiuSistema.Controllers
{
    public class ReportsController : Controller
    {
        [Route("Events/{eventId:int}/Report")]
        public ViewResult OpenReportGenerator(int eventId)
        {
            return View("~/Views/Events/ReportGenView.cshtml", new ReportGenViewModel {EventId = eventId});
        }

        public async Task<ActionResult> CreateReport(ReportGenViewModel model)
        {
            bool valid = CheckValidity(model);

            if (valid)
            {
                Event e = Event.SelectEvent(model.EventId);

                var task1 = Task.Run(() =>
                {
                    List<Ticket> tickets = Ticket.GetEventTickets(e);
                    List<Order> ticketOrders = Order.GetTicketOrders(tickets);
                    List<ApplicationUser> users = Order.GetParticipantById(ticketOrders);
                    return new {tickets, ticketOrders, users };
                });

                var task2 = Task.Run(() =>
                {
                    if (model.TicketsAndFinanceFilter)
                    {
                        int finishedOrders = Order.CountFinishedOrders(model.EventId);
                        float totalPaidSum = Invoice.GetTotalPaidSum(model.EventId);
                        int unfulfilledTickets = CountUnfulfilledTickets(e, finishedOrders);
                        return new { finishedOrders, totalPaidSum, unfulfilledTickets };
                    }

                    return null;
                });

                await Task.WhenAll(task1, task2);

                return new ViewAsPdf("~/Views/Events/PDFView.cshtml", new PDFViewModel
                {
                    TicketsAndFinanceFilter = task2.Result != null,
                    FinishedOrdersCount = task2.Result?.finishedOrders ?? 0,
                    TotalPaidSum = task2.Result?.totalPaidSum ?? 0,
                    UnfulfilledTickets = task2.Result?.unfulfilledTickets ?? 0,
                    tickets =  task1.Result.tickets,
                    orders = task1.Result.ticketOrders,
                    users = task1.Result.users
                }) { FileName = "Report.pdf" };
            }
            else
            {
                return View("~/Views/Events/ReportGenView.cshtml", model);
            }
            
        }

        public static int CountUnfulfilledTickets(Event e, int finishedOrders)
        {
            return e.Tickets.Count() - finishedOrders;
        }

        public bool CheckValidity(ReportGenViewModel model)
        {
            if (model.TicketsAndFinanceFilter == false && model.UsersFilter == false)
            {
                ModelState.AddModelError("UsersFilter", "At least one checkbox must be checked");
                return false;
            }

            return true;
        }
    }
}
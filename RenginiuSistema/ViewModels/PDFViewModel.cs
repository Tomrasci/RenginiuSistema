using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class PDFViewModel
    {
        public int FinishedOrdersCount { get; set; }
        public float TotalPaidSum { get; set; }
        public int UnfulfilledTickets { get; set; }
        public bool TicketsAndFinanceFilter { get; set; }
        public List<Ticket> tickets = new List<Ticket>();
        public List<Order> orders = new List<Order>();
        public List<ApplicationUser> users = new List<ApplicationUser>();
    }
}
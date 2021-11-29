using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class TicketsViewModel
    {
        public Event Event { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
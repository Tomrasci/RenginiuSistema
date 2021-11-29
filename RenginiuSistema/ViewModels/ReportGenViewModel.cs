using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RenginiuSistema.ViewModels
{
    public class ReportGenViewModel
    {
        public bool UsersFilter { get; set; }
        public bool TicketsAndFinanceFilter { get; set; }
        public int EventId { get; set; }
    }
}
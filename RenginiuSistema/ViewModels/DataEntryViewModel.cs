using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class DataEntryViewModel
    {
        public Event Event { get; set; }

        [DisplayName("Payment Method")]
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public Ticket Ticket { get; set; }

        public string Message { get; set; }
        public bool Error { get; set; } = false;
    }
}
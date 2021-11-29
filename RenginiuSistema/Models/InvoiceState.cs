using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RenginiuSistema.Models
{
    public enum InvoiceState
    {
        [Display(Name = "Issued")]
        Issued,
        [Display(Name = "Paid")]
        Paid
    }
}
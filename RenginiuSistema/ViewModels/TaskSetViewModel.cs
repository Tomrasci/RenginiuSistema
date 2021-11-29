using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class TaskSetViewModel
    {
        public List<Event> Events { get; set; }

        public Event CurrentEvent { get; set; }

        public string SuccessMessage { get; set; } = "";
    }
}
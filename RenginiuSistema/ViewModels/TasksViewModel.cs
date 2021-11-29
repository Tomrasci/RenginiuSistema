using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class TasksViewModel
    {
        public Event Event { get; set; }
        public List<OrgTask> Tasks { get; set; }
    }
}
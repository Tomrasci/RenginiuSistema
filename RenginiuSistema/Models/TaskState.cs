using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RenginiuSistema.Models
{
    public enum TaskState
    {
        [Display(Name = "To do")]
        Todo,
        [Display(Name = "In progress")]
        InProgress,
        [Display(Name = "Completed")]
        Completed
    }
}
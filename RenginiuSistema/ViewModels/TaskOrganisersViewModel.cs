using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RenginiuSistema.Models;

namespace RenginiuSistema.ViewModels
{
    public class TaskOrganisersViewModel
    {
        public OrgTask Task { get; set; }
        public List<Organiser> Organisers { get; set; }

        public int SelectedOrganiserId { get; set; }
    }
}
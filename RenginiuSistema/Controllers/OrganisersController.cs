using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RenginiuSistema.Models;
using WebGrease.Css.Extensions;

namespace RenginiuSistema.Controllers
{
    public class OrganisersController : Controller
    {

        [Route("~/Organisers")]
        public ViewResult OpenEventOrganisers()
        {
            var organisers = Organiser.SelectEventOrganisers();
            return View("~/Views/Organisers/EventOrganiserView.cshtml", organisers);
        }

        [Route("~/Organisers/NewEventOrganiserView")]
        public ViewResult OpenEventOrganiserCreation()
        {
            return View("~/Views/Organisers/NewEventOrganiserView.cshtml");
        }


        [HttpPost]
        public ActionResult CreateEventOrganiser(Organiser e)
        {
            bool valid = ValidateEventOrganiserDetails(e);

            if (valid)
            {
                var result = Organiser.CreateEventOrganiser(e);
                var organisers = Organiser.SelectEventOrganisers();
                return RedirectToAction("OpenEventOrganisers");
            }
            else
            {
                return View("~/Views/Organisers/NewEventOrganiserView.cshtml", e);
            }
        }

        public bool ValidateEventOrganiserDetails(Organiser e)
        {
            return TryValidateModel(e);
        }


        [HttpPost]
        [Route("~/Organisers/Remove")]
        public ActionResult RemoveOrganiser(int id)
        {
            Organiser t = Organiser.GetOrganiser(id);
            Organiser.RemoveOrganiser(t);
            List<Organiser> organisers = Organiser.SelectEventOrganisers();
            return View("~/Views/Organisers/EventOrganiserView.cshtml", organisers);
        }

    }
}
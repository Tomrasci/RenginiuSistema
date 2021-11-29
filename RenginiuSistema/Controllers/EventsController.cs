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
    public class EventsController : Controller
    {
        [Route("~/")]
        public ViewResult Index()
        {
            return View("~/Views/Shared/MainView.cshtml");
        }

        [Route("~/Events")]
        public ViewResult OpenEvents()
        {
            var events = Event.SelectEvents();
            return View("~/Views/Events/EventsView.cshtml", events);
        }

        [Route("~/Events/New")]
        public ViewResult OpenEventCreator()
        {
            return View("~/Views/Events/NewEventView.cshtml");
        }

        [HttpPost]
        public ActionResult AddEvent(Event e)
        {
            bool valid = CheckEventValidity(e);

            if (valid)
            {
                var result = Event.InsertEvent(e);
                var events = Event.SelectEvents();
                return RedirectToAction("OpenEvents");
            }
            else
            {
                return View("~/Views/Events/NewEventView.cshtml", e);
            }
        }

        public bool CheckEventValidity(Event e)
        {
            return TryValidateModel(e);
        }

        [Route("~/Events/{id:int}")]
        public ActionResult GetEvent(int id)
        {
            Event e = Event.GetEvent(id);
            return View("~/Views/Events/EventView.cshtml", e);
        }

        [Route("~/Events/Edit/{id:int}")]
        public ViewResult OpenEventEditor(int id)
        {
            Event e = Event.SelectEvent(id);
            return View("~/Views/Events/EditEventView.cshtml", e);
        }

        [HttpPost]
        public ActionResult UpdateEvent(Event e)
        {
            bool valid = CheckEventValidity(e);

            if (valid)
            {
                var result = Event.UpdateEvent(e);
                var events = Event.SelectEvents();
                return RedirectToAction("OpenEvents");
            }
            else
            {
                return View("~/Views/Events/EditEventView.cshtml", e);
            }
        }

        [HttpPost]
        [Route("~/Events/Remove")]
        public ActionResult RemoveEvent(int id)
        {
            Event e = Event.GetEvent(id);
            int timelineId = e.Timeline?.Id ?? -1;
            Event removed = Event.RemoveEvent(e);
            Timeline removedTimeline = Timeline.RemoveAssociatedTimeline(timelineId);
            List<Event> events = Event.SelectEvents();
            return View("~/Views/Events/EventsView.cshtml", events);
        }
    }
}
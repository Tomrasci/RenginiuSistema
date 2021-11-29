using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RenginiuSistema.Models;
using RenginiuSistema.ViewModels;

namespace RenginiuSistema.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        [Route("~/Events/{eventId:int}/Tasks")]
        public ActionResult OpenTasksView(int eventId)
        {
            List<OrgTask> Tasks = OrgTask.SelectEventTasks(eventId);
            Event e = Event.GetEvent(eventId);

            return View("TasksView", new TasksViewModel
            {
                Tasks = Tasks,
                Event = e
            });
        }
        [Route("~/Tasks/{eventId:int}/New")]
        public ViewResult OpenTaskCreation(int eventId)
        {
            OrgTask t = new OrgTask();
            t.Event = Event.GetEvent(eventId);
            return View("~/Views/Tasks/NewTaskView.cshtml", t);
        }


        [HttpPost]
        public ActionResult CreateTask(OrgTask t)
        {
            t.DateCreated = DateTime.Now;
            t.State = TaskState.Todo;
            var eventId = t.Event.Id;

            bool valid = ValidateTaskDetails(t);

            if (valid)
            {
                 var result = OrgTask.CreateTask(t, eventId);
                 var tasks = OrgTask.SelectEventTasks(eventId);
                 return RedirectToRoute("TaskViewUrl", new {id = eventId});
            }
            else
            {
                return View("~/Views/Tasks/NewTaskView.cshtml", t);
            }
        }

        public bool ValidateTaskDetails(OrgTask t)
        {
            return TryValidateModel(t);
        }

        public bool ValidateTaskStateDetails(OrgTask t)
        {
            return TryValidateModel(t);
        }

        [Route("~/Tasks/{eventId:int}/Edit/{id:int}")]
        public ActionResult OpenTaskEdit(int id, int eventId)
        {
            OrgTask t = OrgTask.SelectTask(id);
            t.Event_Id = eventId;
            return View("~/Views/Tasks/EditTaskView.cshtml", t);
        }

        [HttpPost]
        public ActionResult UpdateTask(OrgTask t)
        {
            bool valid = ValidateTaskDetails(t);

            if (valid)
            {
                var result = OrgTask.UpdateTask(t);
                var tasks = OrgTask.SelectEventTasks(t.Event_Id);

                return RedirectToRoute("TaskViewUrl", new {id = t.Event_Id});
            }
            else

            {
                return View("~/Views/Tasks/EditTaskView.cshtml", t);
            }
        }

        [HttpPost]
        [Route("~/Tasks/Remove")]
        public ActionResult RemoveTask(int id)
        {
            OrgTask t = OrgTask.GetTask(id);
            int eventId = t.Event_Id;
            Event e = t.Event;
            OrgTask.RemoveTask(t);
            List<OrgTask> tasks = OrgTask.SelectEventTasks(eventId);
            return View("~/Views/Tasks/TasksView.cshtml",
                new TasksViewModel{Tasks = tasks, Event = e});
        }

        [Route("~/Tasks/{eventId:int}/ChangeState/{id:int}")]
        public ActionResult OpenTaskState(int id, int eventId)
        {
            OrgTask t = OrgTask.SelectTask(id);
            t.Event_Id = eventId;
            return View("~/Views/Tasks/TaskStateView.cshtml", t);
        }

        public ActionResult UpdateTaskState(OrgTask t)
        {
            bool valid = ValidateTaskStateDetails(t);
            if (valid)
            {
                OrgTask.UpdateTaskState(t);
                OrgTask.SelectEventTasks(t.Event_Id);
                return RedirectToRoute("TaskViewUrl", new { id = t.Event_Id });
            }
            else
            {

                return View("~/Views/Tasks/TaskStateView.cshtml", t);
            }
        }
        [Route("Events/{eventId:int}/ApplyTaskSet")]
        public ActionResult OpenOldTaskSetView(int eventId, int? i)
        {
            List<Event> events = Event.GetOldEventsWithTasks(eventId);
            Event current = Event.GetEvent(eventId);
            if (i == 1)
            {
                TaskSetViewModel model = new TaskSetViewModel
                {
                    Events = events, CurrentEvent = current,
                    SuccessMessage = "Successfully added old task set"
                };
                return View("~/Views/Tasks/OldTaskSetsView.cshtml",
                    model);
            }
            return View("~/Views/Tasks/OldTaskSetsView.cshtml",
                new TaskSetViewModel {Events = events, CurrentEvent = current});

        }
        [Route("Events/{currentId:int}/ApplyTaskSet/{eventId:int}")]
        public ActionResult ApplyOldTaskSet(int eventId, int currentId)
        {
            Event e = Event.GetEvent(eventId);
            Event current = Event.GetEvent(currentId);
            foreach (var task in e.Tasks)
            {
                ChangeTaskDatesAndTimes(task);
                ChangeTaskStates(task);
                OrgTask.CreateTask(task, currentId);
            }

            return RedirectToAction("OpenOldTaskSetView", new {eventId=currentId, i=1});
        }
        [Route("~/Tasks/{eventId:int}/Organiser/{taskId:int}")]
        public ActionResult OpenAssignOrganiser(int taskId, int eventId)
        {
            List<Organiser> organisers = Organiser.SelectOrganisers();
            OrgTask task = OrgTask.GetTask(taskId);
            task.Event_Id = eventId;
            return View("~/Views/Tasks/TaskOrganiserView.cshtml",
                new TaskOrganisersViewModel
                {
                    Task = task,
                    Organisers = organisers
                    
                });
        }

        public ActionResult AssignOrganiser(TaskOrganisersViewModel v)
        {
            int OrgId = v.SelectedOrganiserId;
            OrgTask t = v.Task;
            Organiser o = Organiser.GetOrganiser(OrgId);
            bool valid = ValidateOrganiserAssignment(t, o);

            if (valid)
            {
                OrgTask.UpdateTaskOrganiser(t, o);
                return RedirectToRoute("TaskViewUrl", new { id = t.Event_Id });

            }

            else
            {
                List<Organiser> organisers = Organiser.SelectOrganisers();
                return View("~/Views/Tasks/TaskOrganiserView.cshtml", new TaskOrganisersViewModel
                {
                    Task = t,
                    Organisers = organisers
                });
            }
            }

        public bool ValidateOrganiserAssignment(OrgTask t, Organiser o)
        {
            return (TryValidateModel(t) && TryValidateModel(o));
        }




        public void ChangeTaskDatesAndTimes(OrgTask t)
        {
            t.Deadline = null;
            t.DateCompleted = null;
            t.DateCreated = DateTime.Now;
        }

        public void ChangeTaskStates(OrgTask t)
        {
            t.State = TaskState.Todo;
        }
    }
}
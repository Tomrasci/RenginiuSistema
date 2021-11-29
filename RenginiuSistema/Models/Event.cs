using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace RenginiuSistema.Models
{
    public class Event
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Event Name")]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Date of the Event")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayName("Event Setting")]
        public string Setting { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Enable Public Link")]
        public bool IsLinkActive { get; set; } = false;

        [Required]
        [DisplayName("Size of the Event")]
        public int Size { get; set; }

        [DisplayName("Event Budget")]
        public decimal Budget { get; set; }

        [DisplayName("Ticket Price")]
        [Required]
        public decimal TicketPrice { get; set; }

        [DisplayName("Event Address")]
        public string Address { get; set; }

        [DisplayName("Event Contact Number")]
        [Phone]
        public string ContactNumber { get; set; }

        public Timeline Timeline { get; set; }

        public List<Ticket> Tickets { get; set; }

        public List<OrgTask> Tasks { get; set; }

        public static List<Event> SelectEvents()
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Events
                    .Include(e => e.Timeline)
                    .Include(e => e.Tasks)
                    .Include(e => e.Tickets)
                    .ToList();
            }
        }

        public static Event SelectEvent(int id)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Events
                    .Include(e => e.Timeline)
                    .Include(e => e.Tasks)
                    .Include(e => e.Tickets)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public static Event InsertEvent(Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Event inserted = _db.Events.Add(e);
                inserted.Timeline = new Timeline();
                inserted.Tasks = new List<OrgTask>();
                inserted.Tickets = Ticket.CreateTickets(inserted, inserted.Size);
                _db.SaveChanges();
                return inserted;
            }
        }

        public static Event GetEvent(int id)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Events
                    .Include(e => e.Timeline)
                    .Include(e => e.Tasks)
                    .Include(e => e.Tickets)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public static Event UpdateEvent(Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Event eventInDb = _db.Events.SingleOrDefault(x => x.Id == e.Id);

                eventInDb.Name = e.Name;
                eventInDb.Description = e.Description;
                eventInDb.Date = e.Date;
                eventInDb.Address = e.Address;
                eventInDb.IsLinkActive = e.IsLinkActive;
                eventInDb.Setting = e.Setting;
                eventInDb.Size = e.Size;
                eventInDb.Budget = e.Budget;
                eventInDb.ContactNumber = e.ContactNumber;

                _db.SaveChanges();

                return eventInDb;
            }
        }

        public static Event RemoveEvent(Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Events.Attach(e);

                //foreach (var t in e.Tickets.ToList())
                //{
                //    _db.Tickets.Attach(t);
                //    _db.Tickets.Remove(t);
                //}

                List<OrgTask> eventTasks = OrgTask.SelectEventTasks(e.Id);
                foreach (var t in eventTasks)
                {
                    OrgTask.RemoveTask(t);
                }

                Event removed = _db.Events.Remove(e);
                _db.SaveChanges();
                return removed;
            }
        }

        public static List<Event> GetOldEventsWithTasks(int eventId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Event current = GetEvent(eventId);
                List<Event> events = _db.Events

                    .Where(x => x.Id != eventId)
                    .Include(e => e.Tasks.Select(t=>t.Organiser))
                    .ToList();
                List<Event> results =
                     events.Where(p => current.Tasks.All(p2 => p.Tasks.All(p3 => p3.Name != p2.Name))).ToList();
                return results;
            }
        }
    }
}
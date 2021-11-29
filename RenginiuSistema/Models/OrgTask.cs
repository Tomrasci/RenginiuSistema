using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RenginiuSistema.Models
{
    public class OrgTask
    {

        public int Id { get; set; }
        [DisplayName("Task Name")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Task Description")]
        public string Description { get; set; }

        [DisplayName("Task Organiser")]
        [ForeignKey("Organiser_Id")]
        public virtual Organiser Organiser { get; set; }

        public int? Organiser_Id { get; set; }

        [DisplayName("Task State")]
        public TaskState State { get; set; }

        [DisplayName("Created for")]
        [ForeignKey("Event_Id")]
        public Event Event { get; set; }

        [Required]
        public int Event_Id { get; set; }

        [DisplayName("Created On")]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Completed On")]
        public DateTime? DateCompleted { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Task Deadline")]
        public DateTime? Deadline { get; set; }

        public static List<OrgTask> SelectEventTasks(int eventId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Configuration.LazyLoadingEnabled = false;
                return _db.Tasks.Include(t => t.Event)
                    .Where(x => x.Event.Id == eventId).Include(t=>t.Organiser)
                    .ToList();
            }
        }

        public static OrgTask CreateTask(OrgTask t, int eventId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                if (eventId <= 0)
                {
                    throw new Exception("Event ID not provided");
                }

                _db.SaveChanges();
                t.Event = null;
                t.Event_Id = eventId;
                OrgTask inserted = _db.Tasks.Add(t);
                _db.SaveChanges();
                return inserted;
            }
        }

        public static OrgTask UpdateTask(OrgTask t)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                OrgTask selected = _db.Tasks.SingleOrDefault(x => x.Id == t.Id);

                selected.Name = t.Name;
                selected.Description = t.Description;
                selected.Deadline = t.Deadline;
                selected.DateCompleted = t.DateCompleted;

                _db.SaveChanges();

                return selected;
            }
        }

        public static OrgTask GetTask(int taskId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Tasks
                    .Include(t => t.Event).Include(t=>t.Organiser)
                    .SingleOrDefault(t => t.Id == taskId);
            }
        }

        public static OrgTask SelectTask(int taskId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Tasks.FirstOrDefault(x => x.Id == taskId);
            }
        }

        public static void RemoveTask(OrgTask t)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Tasks.Attach(t);
                _db.Tasks.Remove(t);
                _db.SaveChanges();
            }
        }

        public static void UpdateTaskState(OrgTask t)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                OrgTask selected = _db.Tasks.SingleOrDefault(x => x.Id == t.Id);
                selected.State = t.State;

                _db.SaveChanges();
            }
        }

        public static void UpdateTaskOrganiser(OrgTask t, Organiser o)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Tasks.Attach(t);
                t.Organiser = null;
                t.Organiser_Id = o.Id;
                _db.SaveChanges();
            }
        }
    }
}
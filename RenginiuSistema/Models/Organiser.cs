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
    public class Organiser
    {

        private static readonly ApplicationDbContext _db;

        public int Id { get; set; }

        [DisplayName("Organiser Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public List<Event> Events { get; set; }
        public List<OrgTask> Tasks { get; set; }

        public static List<Organiser> SelectEventOrganisers()
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Organisers.ToList();
            }
        }


        public static Organiser CreateEventOrganiser(Organiser e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Organiser inserted = _db.Organisers.Add(e);
                _db.SaveChanges();
                return inserted;
            }
        }

        public static void RemoveOrganiser(Organiser e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Organisers.Attach(e);
                _db.Organisers.Remove(e);
                _db.SaveChanges();
            }

        }

        public static Organiser GetOrganiser(int id)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Organisers
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        public static List<Organiser> SelectOrganisers()
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Organisers.ToList();
            }
        }
    }
}
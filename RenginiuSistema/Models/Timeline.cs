using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RenginiuSistema.Models
{
    public class Timeline
    {

        public int Id { get; set; }

        public static Timeline RemoveAssociatedTimeline(int id)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Timeline removed = null;
                Timeline t = new Timeline {Id = id};
                t = _db.Timelines.Attach(t);
                if (t != null)
                    removed = _db.Timelines.Remove(t);
                _db.SaveChanges();
                return removed;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace RenginiuSistema.Models
{
    public class Invoice
    {

        public int Id { get; set; }
        public float Price { get; set; }
        public InvoiceState State { get; set; }
        public DateTime? DatePaid { get; set; }

        public static Invoice FormAnInvoice(decimal price)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                Invoice i = new Invoice
                {
                    Price = (float) price,
                    State = InvoiceState.Issued
                };

                Invoice added = _db.Invoices.Add(i);

                _db.SaveChanges();

                return added;
            }
        }

        public static Invoice SaveInvoice(Invoice i)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Invoices.Attach(i);
                i.State = InvoiceState.Paid;
                _db.SaveChanges();
                return i;
            }
        }

        public static float GetTotalPaidSum(int eventId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Orders.Include(o => o.Invoice)
                    .Include(o => o.Ticket).Include(o => o.Ticket.Event)
                    .Where(o => o.Ticket.Event.Id == eventId)
                    .Sum(o => (float?)o.Invoice.Price) ?? 0f;
            }
        }
    }
}
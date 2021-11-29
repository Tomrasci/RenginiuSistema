using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RenginiuSistema.Models
{
    public class Ticket
    {
        private static Random _random = new Random();

        public int Id { get; set; }
        public float Price { get; set; }
        public string Code { get; set; }
        public DateTime? DateCreated { get; set; }
        public virtual Event Event { get; set; }

        public static List<Ticket> CreateTickets(Event e, int n)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<Ticket> tickets = new List<Ticket>();
                for (int i = 0; i < n; i++)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    string code = new string(Enumerable.Repeat(chars, 64)
                        .Select(s => s[_random.Next(s.Length)]).ToArray());

                    Ticket ticket = new Ticket
                    {
                        Price = (float) e.TicketPrice,
                        Code = code,
                        DateCreated = DateTime.Now
                    };

                    _db.Tickets.Add(ticket);
                    tickets.Add(ticket);
                }

                return tickets;
            }
        }

        public static Ticket GetUnpurchasedTicket(Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<Ticket> tickets = e.Tickets;
                List<Order> orders = _db.Orders.Include(x => x.Ticket).ToList();

                foreach (var t in tickets)
                {
                    if (!orders.Exists(o => o.Ticket.Id == t.Id))
                        return t;
                }

                _db.SaveChanges();

                throw new Exception();
            }
        }

        public static List<Ticket> SelectTicketsList(List<Order> orders, Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<Ticket> tickets = new List<Ticket>();
                foreach (Order o in orders)
                {
                    if (e.Tickets.Exists(t => t.Id == o.Ticket.Id))
                        tickets.Add(o.Ticket);
                }

                return tickets;
            }
        }

        public static List<Ticket> GetEventTickets(Event e)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<Ticket> tickets = _db.Tickets.Where(x => x.Event.Id == e.Id).ToList();
                List<Ticket> filteredTickets = new List<Ticket>();
                foreach (var ticket in tickets)
                {
                    if (_db.Orders.Include(t => t.Ticket).Where(o => o.State == OrderState.Completed)
                        .Where(e => e.Ticket.Id == ticket.Id).Count() > 0)
                    {
                        filteredTickets.Add((ticket));
                    }
                    
                    

                }

                return filteredTickets;
            }
        }

        public static Ticket SelectTicketInformation(int ticketId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
               return _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            }
        }
    }
}
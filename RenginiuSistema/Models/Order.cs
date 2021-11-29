using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Ast.Selectors;

namespace RenginiuSistema.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public DateTime? DateCreated { get; set; }
        public OrderState State { get; set; }
        public Invoice Invoice { get; set; }
        public ApplicationUser Client { get; set; }
        public virtual Ticket Ticket { get; set; }

        public static Order FormAnOrder(Ticket t, Invoice i)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                string userId = HttpContext.Current.User.Identity.GetUserId();
                t = _db.Tickets.SingleOrDefault(x => x.Id == t.Id);

                Order o = new Order
                {
                    DateCreated = DateTime.Now,
                    Invoice = i,
                    Ticket = t,
                    State = OrderState.Issued,
                    Client = _db.Users.FirstOrDefault(x => x.Id == userId)
                };

                Order added = _db.Orders.Add(o);

                _db.SaveChanges();

                return added;
            }
        }

        public static Order SaveOrder(Order o)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Orders.Attach(o);
                o.State = OrderState.Completed;
                _db.SaveChanges();
                return o;
            }
        }

        public static Order CancelOrder(Order o)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                _db.Orders.Attach(o);
                _db.Orders.Remove(o);
                _db.SaveChanges();
                return o;
            }
        }

        public static List<Order> GetOrders()
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                string userId = HttpContext.Current.User.Identity.GetUserId();

                return _db.Orders.Include(t=>t.Ticket).Where(o => o.Client.Id == userId).ToList();
            }
        }

        public static Order GetOrderForTicket(Ticket t)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                string userId = HttpContext.Current.User.Identity.GetUserId();

                return _db.Orders.Where(o => o.Client.Id == userId).FirstOrDefault(o => o.Ticket.Id == t.Id);
            }
        }

        public static List<Order> GetTicketOrders(List<Ticket> tickets)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<Order> orders = new List<Order>();
                foreach (var ticket in tickets)
                {
                    Order order = _db.Orders.Include(c=>c.Client).Include(e=>e.Ticket).FirstOrDefault(o => o.Ticket.Id == ticket.Id);
                    if (order != null)
                    {
                        orders.Add(order);
                    }
                }
                
                return orders;
            }
        }

        public static List<ApplicationUser> GetParticipantById(List<Order> orders)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                List<ApplicationUser> users = new List<ApplicationUser>();

                foreach (var order in orders)
                {
                    if (order != null)
                    {
                        ApplicationUser user = _db.Users.FirstOrDefault(u => u.Id == order.Client.Id);
                        if (!users.Contains(user)) {
                            users.Add(user);
                        }
                    }
                }

                return users;
            }
        }

        public static int CountFinishedOrders(int eventId)
        {
            using (ApplicationDbContext _db = new ApplicationDbContext())
            {
                return _db.Orders.Include(o => o.Ticket)
                    .Include(o => o.Ticket.Event)
                    .Where(o => o.Ticket.Event.Id == eventId)
                    .Where(o => o.State == OrderState.Completed)
                    .Count();
            }
        }
    }
}
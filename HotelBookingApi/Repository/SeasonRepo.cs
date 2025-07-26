using HotelBooking.Domain.Models;
using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class SeasonRepo:ISeasonRepo
    {
        private readonly AppDbContext db;

        public SeasonRepo(AppDbContext db)
        {
            this.db = db;
        }
        //getall
        public List<Season> GetAll()
        {
            return db.Seasons.Include(se => se.Bookings).ToList();
        }
        //getbyid
        public Season GetById(int id)
        {
            return db.Seasons.Include(se => se.Bookings).FirstOrDefault(se => se.Id == id);
        }
        //add
        public void Add(Season s)
        {
            db.Seasons.Add(s);
        }
        //edit
        public void update(Season s)
        {
            db.Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        //delete
        public void delete(Season s)
        {
            db.Seasons.Remove(s);
        }
        //save
        public void save()
        {
            db.SaveChanges();
        }

    }
}

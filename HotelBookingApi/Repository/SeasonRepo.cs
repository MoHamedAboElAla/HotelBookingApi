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
            return db.Seasons.Include(se => se.Bookings).Include(se => se.Hotel).ToList();
        }
        //getbyid
        public Season GetById(int id)
        {
            return db.Seasons.Include(se => se.Bookings).Include(se => se.Hotel).FirstOrDefault(se => se.Id == id);
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


        //Paged
        public List<Season> GetPaged(int pageNumber, int pageSize)
        {
            return db.Seasons
                .OrderBy(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalCount()
        {
            return db.Seasons.Count();
        }


    }
}

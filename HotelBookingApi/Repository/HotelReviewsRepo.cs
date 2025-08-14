using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Repository
{
    public class HotelReviewsRepo : IHotelReviewsRepo
    {
        private readonly AppDbContext db;

        public HotelReviewsRepo(AppDbContext db)
        {
            this.db = db;
        }
        public List<HotelReview> GetAll()
        {
            return db.HotelReviews.Include(a => a.Agent).Include(h => h.Hotel).ToList();
        }

        public HotelReview GetById(int id)
        {
            return db.HotelReviews.Include(a => a.Agent).Include(h => h.Hotel).FirstOrDefault(r => r.Id == id)!;
        }

        public void Add(HotelReview r)
        {
            db.HotelReviews.Add(r);
        }
        public void update(HotelReview r)
        {
            db.Entry(r).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void delete(HotelReview r)
        {
            db.HotelReviews.Remove(r);
        }
        //save
        public void save()
        {
            db.SaveChanges();
        }
    }
}

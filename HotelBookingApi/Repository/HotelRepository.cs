using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;

namespace HotelBookingApi.Repository
{
    public class HotelRepository : IHotelRepository
    {
        AppDbContext _dbcontext;

        public HotelRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public void add(Hotel hotel)
        {
            _dbcontext.Hotels.Add(hotel);
        }

        public void edit(Hotel hotel)
        {
            _dbcontext.Entry(hotel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public List<Hotel> GetAll()
        {
            return _dbcontext.Hotels.ToList();
        }

        public Hotel GetById(int id)
        {
            return _dbcontext.Hotels.Find(id);
        }

        public void remove(int id)
        {
           Hotel hotel = _dbcontext.Hotels.Find(id);
            _dbcontext.Hotels.Remove(hotel);
        }

        public void save()
        {
            _dbcontext.SaveChanges();
        }
    }
}

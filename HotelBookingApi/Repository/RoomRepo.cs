using HotelBooking.Domain.Models;
using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;

namespace HotelBookingApi.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private AppDbContext db;

       public RoomRepo (AppDbContext db)
        {
            this.db = db;
        }

        public List<Room> GetAll()
        {
            return db.Rooms.ToList();

        }
        public Room GetbyId(int id)
        {
            return db.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public void Add(Room r)
        {
            db.Rooms.Add(r);

        }
        public void Update(Room r)
        {
            db.Entry(r).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        }
        public void Delete(Room r)
        {
            db.Rooms.Remove(r);
        }
        public void Save()
        {
            db.SaveChanges();
        }


    }
}

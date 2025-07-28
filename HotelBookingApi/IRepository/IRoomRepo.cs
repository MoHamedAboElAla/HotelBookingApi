
using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IRoomRepo
    {
        public List<Room> GetAll();
        public Room GetbyId(int id);
        public void Add(Room r);
        public void Update(Room r);
        public void Delete(Room r);
       
        public void Save();




    }
}

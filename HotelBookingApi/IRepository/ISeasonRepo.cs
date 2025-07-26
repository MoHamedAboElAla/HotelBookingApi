using HotelBooking.Domain.Models;

namespace HotelBookingApi.IRepository
{
    public interface ISeasonRepo
    {
        //getall
        public List<Season> GetAll();
        //getbyid
        public Season GetById(int id);
        //add
        public void Add(Season s);
        //edit
        public void update(Season s);
        //delete
        public void delete(Season s);
        //save
        public void save();
    }
}

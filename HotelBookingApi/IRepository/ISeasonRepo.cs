
using HotelBookingApi.Models;

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
        Task<Season?> GetSeasonByDateRangeAsync(DateTime checkIn, DateTime checkOut);

        //save
        public void save();

        List<Season> GetPaged(int pageNumber, int pageSize);
        int GetTotalCount();

    }
}

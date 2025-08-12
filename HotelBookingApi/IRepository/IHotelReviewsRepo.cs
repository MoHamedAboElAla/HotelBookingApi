using HotelBookingApi.Models;

namespace HotelBookingApi.IRepository
{
    public interface IHotelReviewsRepo
    {
        //getall
        public List<HotelReview> GetAll();
        //getbyid
        public HotelReview GetById(int id);
        //add
        public void Add(HotelReview r);
        //edit
        public void update(HotelReview r);
        //delete
        public void delete(HotelReview r);

        //save
        public void save();

    }
}

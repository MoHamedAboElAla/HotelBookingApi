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
        /*
                public List<Hotel> SearchAndPaginate(string? searchTerm, int page, int pageSize, out int totalCount)
                {
                    var query = _dbcontext.Hotels.AsQueryable();

                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        query = query.Where(h => h.Name!.Contains(searchTerm) || h.Country!.Contains(searchTerm));
                    }

                    totalCount = query.Count();

                    return query
                        .OrderBy(h => h.Name)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                */
        public List<Hotel> SearchAndPaginate(
            string? searchTerm,
            int page,
            int pageSize,
            string sortBy,
            string sortDirection,
            out int totalCount)
        {
            var query = _dbcontext.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(h => h.Name!.Contains(searchTerm) || h.Country!.Contains(searchTerm));
            }

            // Order
            query = (sortBy.ToLower(), sortDirection.ToLower()) switch
            {
                ("name", "asc") => query.OrderBy(h => h.Name),
                ("name", "desc") => query.OrderByDescending(h => h.Name),
                ("stars", "asc") => query.OrderBy(h => h.Stars),
                ("stars", "desc") => query.OrderByDescending(h => h.Stars),
                _ => query.OrderBy(h => h.Name) 
            };

            totalCount = query.Count();

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

    }
}

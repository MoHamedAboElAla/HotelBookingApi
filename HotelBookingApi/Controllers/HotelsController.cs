using HotelBookingApi.Dtos;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        IHotelRepository _repo;
        private readonly IWebHostEnvironment _env;
        public HotelsController(IHotelRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }
        [HttpGet]
        public IActionResult GetAllHotels()
        {
            var Hotels = _repo.GetAll();
            if (Hotels == null)
                return NotFound("No Hotels Found");
            return Ok(Hotels);
        }
        // search and pagination
        [HttpGet("search")]
        public IActionResult SearchHotels(
          [FromQuery] string? term,
          [FromQuery] int page = 1,
          [FromQuery] int pageSize = 10,
          [FromQuery] string sortBy = "name",
          [FromQuery] string sortDirection = "asc")
        {
            var hotels = _repo.SearchAndPaginate(term, page, pageSize, sortBy, sortDirection, out int totalCount);

            var result = new
            {
                Data = hotels,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortDirection = sortDirection
            };

            return Ok(result);
        }

        /*
        [HttpGet("search")]
        public IActionResult SearchHotels([FromQuery] string? term, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var hotels = _repo.SearchAndPaginate(term, page, pageSize, out int totalCount);

            var result = new
            {
                Data = hotels,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }
        */
        // return all hotel with pagination without search
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return SearchHotels(null, page, pageSize);
        }
        [HttpGet("{id}")]
        public IActionResult GetHotelById(int id)
        {
            var hotel = _repo.GetById(id);
            if (hotel == null)
                return NotFound($"hotel with id {id} not found");
            return Ok(hotel);
        }
        [HttpPost]
        public IActionResult Post([FromForm] HotelDto hotelDto)
        {
            if (hotelDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return BadRequest(ModelState);
            }

            string imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                   Path.GetExtension(hotelDto.ImageFile.FileName);

            string imagesFolder = Path.Combine(_env.WebRootPath, "images", "hotel");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            string fullPath = Path.Combine(imagesFolder, imageFileName);

            using (var stream = System.IO.File.Create(fullPath))
            {
                hotelDto.ImageFile.CopyTo(stream);
            }

            var hotel = new Hotel()
            {
                Name = hotelDto.Name,
                Description = hotelDto.Description ?? "",
                Location = hotelDto.Location,
                Country = hotelDto.Country,
                Stars = hotelDto.Stars,
                ImageFileName = imageFileName
            };

            _repo.add(hotel);
            _repo.save();
            return Ok(hotel);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] HotelDto hotelDto)
        {
            var hotel = _repo.GetById(id);
            if (hotel == null)
            {
                return NotFound();
            }
            string imageFileName = hotel.ImageFileName;
            if (hotelDto.ImageFile != null)
            {
              //  Save Image on the Server
                imageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                imageFileName += Path.GetExtension(hotelDto.ImageFile.FileName);

                string imagesFolder = _env.WebRootPath + "/images/hotel";
                using (var stream = System.IO.File.Create(imagesFolder + imageFileName))
                {
                    hotelDto.ImageFile.CopyTo(stream);
                }
                //Delete Old Image
                  System.IO.File.Delete(imagesFolder + hotel.ImageFileName);
            }
            //Update Hotel
            hotel.Name = hotelDto.Name;
            hotel.Description = hotelDto.Description ?? "";
            hotel.Location = hotelDto.Location;
            hotel.Country = hotelDto.Country;
            
            hotel.ImageFileName = imageFileName;
            _repo.save();
         
            return Ok(hotel);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            Hotel hotel = _repo.GetById(id);
            if (hotel == null)
             return NotFound();
            _repo.remove(id);
            _repo.save();     
            return Ok(hotel);
        }
    }
}

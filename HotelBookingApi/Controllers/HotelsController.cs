using AutoMapper;
using HotelBookingApi.Dtos;
using HotelBookingApi.IRepository;
using HotelBookingApi.Models;
using HotelBookingApi.Repository;
using HotelBookingApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
  

        private readonly IHotelRepository _repo;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IImageUrlService _imageUrlService;

        public HotelsController(IHotelRepository repo, IWebHostEnvironment env, IMapper mapper, IImageUrlService imageUrlService)
        {
            _repo = repo;
            _env = env;
            _mapper = mapper;
            _imageUrlService = imageUrlService;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _repo.GetAllAsync();
            if (hotels == null || !hotels.Any())
                return NotFound("No Hotels Found");
            return Ok(hotels);
        }
        */
        /*
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _repo.GetAllAsync();

            if (hotels == null || !hotels.Any())
                return NotFound("No Hotels Found");

            var result = _mapper.Map<List<HotelViewDto>>(hotels);

            foreach (var (dto, hotel) in result.Zip(hotels))
            {
                dto.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/hotel/{hotel.ImageFileName}";
            }

            return Ok(result);
        }
        */
        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _repo.GetAllAsync();
            if (hotels == null || !hotels.Any())
                return NotFound("No Hotels Found");

            var result = _mapper.Map<List<HotelViewDto>>(hotels);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].ImageUrl = _imageUrlService.GenerateHotelImageUrl(hotels[i].ImageFileName!);
            }

            return Ok(result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchHotels(
            [FromQuery] string? term,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "name",
            [FromQuery] string sortDirection = "asc")
        {
            var (hotels, totalCount) = await _repo.SearchAndPaginateAsync(term, page, pageSize, sortBy, sortDirection);

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

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return await SearchHotels(null, page, pageSize);
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null)
                return NotFound($"Hotel with id {id} not found");
            return Ok(hotel);
        }
        */
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null)
                return NotFound();

            var dto = _mapper.Map<HotelViewDto>(hotel);
            dto.ImageUrl = $"{Request.Scheme}://{Request.Host}/images/hotel/{hotel.ImageFileName}";

            return Ok(dto);
        }
        */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null)
                return NotFound("Hotel not found");

            var hotelDto = _mapper.Map<HotelViewDto>(hotel);
            hotelDto.ImageUrl = _imageUrlService.GenerateHotelImageUrl(hotel.ImageFileName!);

            return Ok(hotelDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
                await hotelDto.ImageFile.CopyToAsync(stream);
            }

            // Data Mapping
            var hotel = _mapper.Map<Hotel>(hotelDto);
            hotel.ImageFileName = imageFileName;

            await _repo.AddAsync(hotel);
            await _repo.SaveAsync();

            return Ok(hotel); 
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] HotelDto hotelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null)
                return NotFound();
            //update all data without image
            _mapper.Map(hotelDto, hotel); 

            if (hotelDto.ImageFile != null)
            {
                string imagesFolder = Path.Combine(_env.WebRootPath, "images", "hotel");

                //  delete old image
                string oldImagePath = Path.Combine(imagesFolder, hotel.ImageFileName);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                //  save new image
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                     Path.GetExtension(hotelDto.ImageFile.FileName);
                string fullPath = Path.Combine(imagesFolder, newFileName);

                using (var stream = System.IO.File.Create(fullPath))
                {
                    await hotelDto.ImageFile.CopyToAsync(stream);
                }

                hotel.ImageFileName = newFileName;
            }

            await _repo.EditAsync(hotel);
            await _repo.SaveAsync();

            return Ok(hotel); 
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _repo.GetByIdAsync(id);
            if (hotel == null)
                return NotFound();

            await _repo.RemoveAsync(id);
            await _repo.SaveAsync();

            return Ok(hotel);
        }
    }
}

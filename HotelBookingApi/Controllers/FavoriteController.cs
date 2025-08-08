using HotelBookingApi.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteRepo _favoriteRepo;

        public FavoriteController(IFavoriteRepo favoriteRepo)
        {
            _favoriteRepo = favoriteRepo;
        }
        private int GetAgentIdFromToken()
        {
            var agentIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);

            if (agentIdClaim == null)
                throw new UnauthorizedAccessException("Invalid token: Agent ID not found.");

            return int.Parse(agentIdClaim.Value);
        }

        [HttpPost("{roomId}")]
        public async Task<IActionResult> AddToFavorites(int roomId)
        {
            var userId = GetAgentIdFromToken(); 
            await _favoriteRepo.AddToFavoritesAsync(userId, roomId);
            return Ok();
        }


        [HttpDelete("{roomId}")]
        public async Task<IActionResult> RemoveFromFavorites(int roomId)
        {
            var userId = GetAgentIdFromToken();
            await _favoriteRepo.RemoveFromFavoritesAsync(userId, roomId);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userId = GetAgentIdFromToken();
            var rooms = await _favoriteRepo.GetFavoritesAsync(userId);
            return Ok(rooms); 
        }

        [HttpGet("is-favorite/{roomId}")]
        public async Task<IActionResult> IsFavorite(int roomId)
        {
            var userId = GetAgentIdFromToken();
            var isFav = await _favoriteRepo.IsFavoriteAsync(userId, roomId);
            return Ok(isFav);
        }
    }
}

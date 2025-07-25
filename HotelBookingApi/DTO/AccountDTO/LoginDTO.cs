using System.ComponentModel.DataAnnotations;

namespace HotelBookingApi.DTO.AccountDTO
{
    public class LoginDTO
    {

            public string UserName { get; set; }

            [DataType(DataType.Password)]
            public string Password { get; set; }

    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class UsersController : Controller
    {
        private List<User> myUsers;

        public UsersController(Data data)
        {
            myUsers = data.Users;
        }

        [HttpGet("users/index")]
        public IActionResult Index()
        {
            return Unauthorized();
        }

        [HttpPost("users/index")]
        public IActionResult Index([FromForm] string id)
        {
            return id == "df2323eoT" ? View(myUsers) : Unauthorized();
        }

    }
}

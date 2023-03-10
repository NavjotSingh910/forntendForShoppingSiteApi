using LoginPageTest.Data;
using LoginPageTest.Dto;
using LoginPageTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace LoginPageTest.Controllers
{
    public class LoginController : Controller
    {

        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> LoginPage(UserLoginDTO user)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7105/api/user/login", user);

            if (response.IsSuccessStatusCode)
            {
                var tokenraw = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWToken", tokenraw);
                    return RedirectToAction("Index", "Pages");
              
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ViewBag.ErrorMessage = errorMessage;

                return View();
            }
        }


     
    }
}



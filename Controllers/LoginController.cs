using LoginPageTest.Data;
using LoginPageTest.Dto;
using LoginPageTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
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
                var tokenString = await response.Content.ReadAsStringAsync();

                HttpContext.Session.SetString("JWToken", tokenString);
                // Decode the token to access the claims
                var handler = new JwtSecurityTokenHandler();
                var decodedToken = handler.ReadJwtToken(tokenString);

                // Check if the "role" claim is "admin"
                if (decodedToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role && claim.Value == "Admin") != null)
                {
                    return RedirectToAction("AdminHome", "Pages");
                }

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

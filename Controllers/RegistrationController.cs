using LoginPageTest.Data;
using LoginPageTest.Dto;
using LoginPageTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace LoginPageTest.Controllers
{
    public class RegistrationController : Controller

    {
        private readonly HttpClient _httpClient;

        public RegistrationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]

        public IActionResult RegistrationPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SuccessPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationPage(ResgisterUser user)
        {

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7105/api/user/register", user);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Successfully done";
                return RedirectToAction("LoginPage", "Login");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                //ModelState.AddModelError("", errorMessage);
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
        }

    }
}

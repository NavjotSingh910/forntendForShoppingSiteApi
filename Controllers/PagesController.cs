using LoginPageTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace LoginPageTest.Controllers
{
    public class PagesController : Controller
    {
        private readonly HttpClient _httpClient;
        public static string baseUrl = "https://localhost:7105/api/items/itemsget";
        public PagesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> AdminHome(string ItemName)
        {
            IEnumerable<Items> products;
            if (string.IsNullOrEmpty(ItemName))
            {
                // No search query submitted, get all products
                products = await GetProducts();
            }
            else
            {
                // Search for products that match the keyword
                products = await SearchItem(ItemName);
            }
            return View(products);
        }

        public async Task<IActionResult> Search(string ItemName)
        {

            var products = await SearchItem(ItemName);

            return View(products);
        }





        [HttpPost]
        public async Task<IActionResult> AdminAddItem(Items model)
        {

            // do something with the data

            
            var accessToken = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7105/api/items/itemAdd", model);




            if (response.IsSuccessStatusCode)
            {
                ViewBag.ErrorMessage = "Successfully done";
                return RedirectToAction("AdminHome");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                //ModelState.AddModelError("", errorMessage);
                ViewBag.ErrorMessage = errorMessage;
                return View();
            }
        }

        [HttpPost]

        public async Task<IActionResult> AdminDeleteItem(int id)
        {
            // Use the id to delete the item from your database
            // ...
            var idd = id;
            var accessToken = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7105/api/items/itemDelete",id);
            return RedirectToAction("AdminHome");
        }

        [HttpGet]
        public async Task<List<Items>> SearchItem(string ItemName)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://localhost:7105/api/items/search?keyword={ItemName}";
            var respose = await _httpClient.GetAsync(url);
            var res = JsonConvert.DeserializeObject<List<Items>>(await respose.Content.ReadAsStringAsync());
            return res;
        }

        [HttpGet]

        public async Task<List<Items>> GetProducts()
        {
            var accessToken= HttpContext.Session.GetString("JWToken");
            var url = baseUrl;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);

            string jsonStr = await _httpClient.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<List<Items>>(jsonStr).ToList();
            return res;


        }

       
    }
}

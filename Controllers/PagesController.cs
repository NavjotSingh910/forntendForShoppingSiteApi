using LoginPageTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Text.Json;
using System.Net.Http.Json;

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

        public async Task<IActionResult> Index()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return View();

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

            // convert the model to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(model);

            // do something with the JSON string
            Console.WriteLine(json);

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
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7105/api/items/itemDelete", id);
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
        static int add = 0;
        [HttpGet]
        public async Task<List<Items>> moreLoad(int startIndex = 0, int count = 10)
        {
            startIndex = add;
            var accessToken = HttpContext.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://localhost:7105/api/items/load?startIndex={startIndex}&count={count}";
            var jsonStr = await _httpClient.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<List<Items>>(jsonStr).ToList();
            add += 9;
            return res;
        }

        [HttpGet]

        public async Task<List<Items>> GetProducts()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string jsonStr = await _httpClient.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<List<Items>>(jsonStr).ToList();
            return res;
        }

        [HttpGet]
        public async Task<IActionResult> AdminApprove()
        {
            IEnumerable<User> products;

            // No search query submitted, get all products
            products = await GetUsers();

            return View(products);
        }
        [HttpGet]

        public async Task<List<User>> GetUsers()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = "https://localhost:7105/api/user/GetUsers";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string jsonStr = await _httpClient.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<List<User>>(jsonStr).ToList();
            return res;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(String id, string status)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = $"https://localhost:7105/api/User/EditStatus?id={id}&status={status}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            return RedirectToAction("AdminApprove");
        }

    }
}

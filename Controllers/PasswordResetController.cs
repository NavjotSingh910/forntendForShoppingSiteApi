using System.Net;
using Microsoft.AspNetCore.Mvc;
using LoginPageTest.Dto;

public class PasswordResetController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PasswordResetController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string email, string username)
    {
        var forgetPassword = new ForgetPassword { Email = email, Username = username };


        var response = await _httpClient.PostAsJsonAsync($"{_configuration["ApiUrl"]}passwordreset/forgetpassword", forgetPassword);

        if (response.IsSuccessStatusCode)
        {
            ViewBag.Success = true;
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            ModelState.AddModelError("NotFound", "User not found.");
        }
        else
        {
            ModelState.AddModelError("Error", "An error occurred while processing your request.");
        }

        return View();
    }

    [HttpGet("/passwordreset/reset-password")]
    public IActionResult ResetPassword(string token)
    {
        return View(new ResetPassword { Token = token });
    }

    [HttpPost("/passwordreset/reset-password")]
    public async Task<IActionResult> GetActionResultAsync(ResetPassword resetPassword)
    {
       
        var response = await _httpClient.PutAsJsonAsync($"{_configuration["ApiUrl"]}passwordreset/ResetPasword", resetPassword);

        if (response.IsSuccessStatusCode)
        {
            ViewBag.Success = true;
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            ModelState.AddModelError("NotFound", "Token invalid or expired.");
        }
        else
        {
            ModelState.AddModelError("Error", "An error occurred while processing your request.");
        }

        return View("Index",resetPassword);
    }
}

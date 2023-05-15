using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Item3.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Item3.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(8);
        List<Employees> list = new List<Employees>();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("http://localhost:5246/api/Employees"),
            Headers =
            {
                {
                    "accept", "application/json"
                }
            }
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Employees>>(body);
            foreach (var item in data)
            {
                Employees obj = new Employees()
                {
                    EmployeeID = item.EmployeeID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Age = item.Age
                };
                list.Add(obj);
            }
        }
        return View(list);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        ViewBag.EmployeeID = id;
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Employees obj)
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(8);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri("http://localhost:5246/api/Employees/" + obj.EmployeeID),
            Headers =
            {
                {
                    "accept", "application/json"
                }
            },
            Content = new StringContent("{\"firstName\":\"" + obj.FirstName + "\",\"lastName\":\"" + obj.LastName + "\",\"age\":" + obj.Age + "}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Employees obj)
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(8);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("http://localhost:5246/api/Employees"),
            Headers =
            {
                {
                    "accept", "application/json"
                }
            },
            Content = new StringContent("{\"employeeID\":0,\"firstName\":\"" + obj.FirstName + "\",\"lastName\":\"" + obj.LastName + "\",\"age\":" + obj.Age + "}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteAsync(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(8);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri("http://localhost:5246/api/Employees/" + id),
            Headers =
            {
                {
                    "accept", "application/json"
                }
            }
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
        }
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

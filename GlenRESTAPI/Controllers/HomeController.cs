using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GlenRESTAPI.Models;
using Newtonsoft.Json;

namespace GlenRESTAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<ActionResult> Index()
        {
            List<Album> AlbumInfo = new List<Album>();
            List<User> UserInfo = new List<User>();
            List<Photo> PhotoInfo = new List<Photo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage albumResp = await client.GetAsync("albums");
                HttpResponseMessage userResp = await client.GetAsync("users");
                HttpResponseMessage photoResp = await client.GetAsync("photos");

                if (albumResp.IsSuccessStatusCode && userResp.IsSuccessStatusCode && photoResp.IsSuccessStatusCode)
                {
                    var AlbumResponse = albumResp.Content.ReadAsStringAsync().Result;
                    var UserResponse = userResp.Content.ReadAsStringAsync().Result;
                    var PhotoResponse = photoResp.Content.ReadAsStringAsync().Result;

                    AlbumInfo = JsonConvert.DeserializeObject<List<Album>>(AlbumResponse);
                    UserInfo = JsonConvert.DeserializeObject<List<User>>(UserResponse);
                    PhotoInfo = JsonConvert.DeserializeObject<List<Photo>>(PhotoResponse);
                    foreach (Album album in AlbumInfo)
                    {
                        foreach (User user in UserInfo)
                        {
                            if (user.id == album.userId)
                            {
                                album.name = user.name;
                                album.email = user.email;
                                album.phone = user.phone;
                                album.address = user.address;
                            }
                        }
                        foreach (Photo photo in PhotoInfo)
                        {
                            if (photo.albumId == album.id)
                            {
                                album.firstThumb = photo.url;
                                break;
                            }
                        }
                    }

                }
                return View(AlbumInfo);
            }
        }

        public async Task<IActionResult> Album(int? id)
        {

            List<Photo> PhotoInfo2 = new List<Photo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage photoResp2 = await client.GetAsync("photos?albumId=" + id);

                if (photoResp2.IsSuccessStatusCode)
                {
                    var PhotoResponse2 = photoResp2.Content.ReadAsStringAsync().Result;
                    PhotoInfo2 = JsonConvert.DeserializeObject<List<Photo>>(PhotoResponse2);
                }

                return View(PhotoInfo2);
            }
        }

        public async Task<IActionResult> Person(int? id)
        {
            List<User> UserInfo2 = new List<User>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage userResp2 = await client.GetAsync("users?id=" + id);

                if (userResp2.IsSuccessStatusCode)
                {
                    var UserResponse2 = userResp2.Content.ReadAsStringAsync().Result;
                    UserInfo2 = JsonConvert.DeserializeObject<List<User>>(UserResponse2);
                }

                return View(UserInfo2);
            }
        }

        public async Task<IActionResult> Posts(int? id)
        {
            List<Post> PostInfo = new List<Post>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage postResp = await client.GetAsync("posts?userId=" + id);

                if (postResp.IsSuccessStatusCode)
                {
                    var PostResponse = postResp.Content.ReadAsStringAsync().Result;
                    PostInfo = JsonConvert.DeserializeObject<List<Post>>(PostResponse);
                }

                return View(PostInfo);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using App.UI.Exceptions;
using App.UI.Filters;
using App.UI.Models;
using App.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace App.UI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class FlowerController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly HttpClient _client;

        public FlowerController(ICrudService crudService, HttpClient client)
        {
            _crudService = crudService;
            _client = client;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                return View(await _crudService.GetAllPaginated<FlowerListItemGetResponse>("flowers", page));
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("login", "account");
                }   
                else
                {
                    return RedirectToAction("error", "home");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("error", "home");
            }
        }

        public ActionResult Create()
        {
            var categories = GetAllCategories().Result;
            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(FlowerCreateRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                var categories = await GetAllCategories();
                ViewBag.Categories = categories;
                return View(createRequest);
            }

            try
            {
                await _crudService.CreateFromForm<FlowerCreateRequest>(createRequest, "flowers");
                return RedirectToAction("index");
            }
            catch (ModelException e)
            {
                foreach (var item in e.Error.Errors)
                {
                    ModelState.AddModelError(item.Key, item.Message);
                }
                var categories = await GetAllCategories();
                ViewBag.Categories = categories;
                return View(createRequest);
            }
            catch (HttpException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var categories = await GetAllCategories();
                ViewBag.Categories = categories;
                return View(createRequest);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Unexpected Error: {e.Message}");
                var categories = await GetAllCategories();
                ViewBag.Categories = categories;
                return View(createRequest);
            }
        }



        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete("flowers/" + id);
                return Ok();
            }
            catch (HttpException e)
            {
                return StatusCode((int)e.Status);
            }
        }

     

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var flower = await _crudService.Get<FlowerGetResponse>("flowers/" + id);
                var editRequest = new FlowerEditRequest
                {
                    Name = flower.Name,
                    Desc = flower.Desc,
                    Price = flower.Price,
                    CategoryIds = flower.Categories.Select(x=>x.Id).ToList(),
                };

                ViewBag.Categories = await GetAllCategories();
                ViewBag.Photos = flower.Photos;

                return View(editRequest);
            }
            catch (HttpException e)
            {
                if (e.Status == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    return RedirectToAction("error", "home");
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return RedirectToAction("error", "home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FlowerEditRequest editRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await GetAllCategories();
                return View(editRequest);
            }
            try
            {
                var editDto = new FlowerEditRequest
                {
                    Name = editRequest.Name,
                    Price = editRequest.Price,
                    Desc = editRequest.Desc,
                    CategoryIds = editRequest.CategoryIds,
                    NewPhotos = editRequest.NewPhotos ?? new List<IFormFile>(),
                    RemovingPhotosIds = editRequest.RemovingPhotosIds,

                };
                await _crudService.UpdateFromForm<FlowerEditRequest>(editRequest, $"flowers/{id}");
                return RedirectToAction("index");
            }
            catch (ModelException e)
            {
                foreach (var item in e.Error.Errors)
                {
                    ModelState.AddModelError(item.Key, item.Message);
                }

                ViewBag.Categories = await GetAllCategories();
                return View(editRequest);
            }
            catch (HttpException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Categories = await GetAllCategories();
                return View(editRequest);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Unexpected Error: {e.Message}");
                ViewBag.Categories = await GetAllCategories();
                return View(editRequest);
            }
        }


        private async Task<List<CategoryListItemGetResponse>> GetAllCategories()
        {
            using (var response = await _client.GetAsync("https://localhost:44361/api/Categories/all"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<List<CategoryListItemGetResponse>>(await response.Content.ReadAsStringAsync(), options);

                    return data;
                }
            }
            return null;
        }

    }
}

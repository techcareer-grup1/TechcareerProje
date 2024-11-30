using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Dtos.Categories.Requests;
using TechCareer.Service.Abstracts;

namespace WebMvc.Controllers
{
    public class CategoriesController(ICategoryService _categoryService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            var categories =  await _categoryService.GetAllAsync();

            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddRequestDto dto)
        {
            await _categoryService.AddAsync(dto);

            return RedirectToAction("Index");
        }


        [HttpGet]
        public  IActionResult Add()
        {
            return View();
        }
    }
}

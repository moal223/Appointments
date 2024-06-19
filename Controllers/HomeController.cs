using Appointement.Interfaces;
using Appointement.Models;
using Appointement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Appointement.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAppointmentRepository _appRepo;

        public HomeController(ILogger<HomeController> logger, IAppointmentRepository appRepo, UserManager<ApplicationUser> userManager)
            :base(userManager)
        {
            _logger = logger;
            _appRepo = appRepo;
        }

        public async Task<IActionResult> Index()
        {
            // get appointements of the current user
            var result = await _appRepo.GetByUserIdAsync(_currentUser.Id);

            ViewData["models"] = result.Count > 0 ? result : new List<Appointment>();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointement([FromForm] CreateAppointementViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Title))
                {
                    await _appRepo.Add(new Appointment { Title = model.Title, Details = model.Details, ExpiDate = model.ExpiDate, ApplicationUserId = _currentUser.Id, ApplicationUser = _currentUser });
                    return RedirectToAction(nameof(Index));
                }
                else
                    ModelState.AddModelError("Title", "Please write the title");
            }
            ViewData["models"] = await _appRepo.GetByUserIdAsync(_currentUser.Id); ;
            return View(nameof(Index), new Appointment { Title = model.Title, Details = model.Details, ExpiDate = model.ExpiDate });
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _appRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Get(int id)
        {
            var result = await _appRepo.GetByUserIdAsync(_currentUser.Id);
            ViewData["models"] = result;
            var res = result.FirstOrDefault(a => a.Id == id);

            return View(nameof(Index), res);
        }
        public async Task<IActionResult> Update(int id, [FromForm] Appointment entity)
        {
            entity.Id = id;
            if (!string.IsNullOrEmpty(entity.Title))
            {
                await _appRepo.Update(entity);
            }
            else
                ModelState.AddModelError("Title", "Please write the title");

            ViewData["models"] = await _appRepo.GetByUserIdAsync(_currentUser.Id);
            return View(nameof(Index), entity);
        }
        public async Task<IActionResult> Search(string key)
        {
            var result = await _appRepo.GetByUserIdAsync(_currentUser.Id);
            if (!string.IsNullOrEmpty(key))
            {
                result = await _appRepo.GetByTitle(key, _currentUser.Id);
                if (result.Count > 0)
                    ViewData["models"] = result;
                else
                    ViewData["models"] = new List<Appointment>();
            }
            else
                ViewData["models"] = result;
            return View(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

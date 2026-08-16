using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models.ViewModels.Medicine;

namespace PharmacyManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString, int? categoryId)
        {
            var medicinesQuery = _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Stocks)
                .Where(m => m.IsActive);

            if (!string.IsNullOrEmpty(searchString))
            {
                medicinesQuery = medicinesQuery.Where(m => m.Name.Contains(searchString) ||
                                                           (m.GenericName != null && m.GenericName.Contains(searchString)) ||
                                                           (m.BrandName != null && m.BrandName.Contains(searchString)));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                medicinesQuery = medicinesQuery.Where(m => m.CategoryId == categoryId.Value);
            }

            var medicines = await medicinesQuery
                .Select(m => new MedicineViewModel
                {
                    MedicineId = m.MedicineId,
                    Name = m.Name,
                    GenericName = m.GenericName,
                    BrandName = m.BrandName,
                    Strength = m.Strength,
                    DosageForm = m.DosageForm,
                    CategoryName = m.Category != null ? m.Category.Name : string.Empty,
                    CategoryId = m.CategoryId,
                    Manufacturer = m.Manufacturer,
                    ImageUrl = m.ImageUrl,
                    IsPrescriptionRequired = m.IsPrescriptionRequired,
                    Description = m.Description,
                    IsActive = m.IsActive,
                    TotalStockQuantity = m.Stocks.Sum(s => s.Quantity),
                    LatestSellingPrice = m.Stocks.OrderByDescending(s => s.CreatedAt).Select(s => s.SellingPrice).FirstOrDefault()
                })
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = categoryId;

            return View(medicines);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
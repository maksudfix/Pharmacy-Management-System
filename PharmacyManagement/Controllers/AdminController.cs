using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models.ViewModels.Admin;
using PharmacyManagement.Models.ViewModels.Customer;
using PharmacyManagement.Models.ViewModels.Medicine;
using PharmacyManagement.Models.ViewModels.Prescription;
using PharmacyManagement.Models.ViewModels.Stock;
using PharmacyManagement.Models.ViewModels.Purchase;

namespace PharmacyManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var customerList = await _context.Customers
                .Include(c => c.Purchases)
                    .ThenInclude(p => p.PurchaseItems)
                .ToListAsync();

            var customers = new List<CustomerListViewModel>();
            foreach (var c in customerList)
            {
                int totalMeds = 0;
                if (c.Purchases != null)
                {
                    foreach (var p in c.Purchases)
                    {
                        if (p.PurchaseItems != null)
                        {
                            foreach (var pi in p.PurchaseItems)
                            {
                                totalMeds += pi.Quantity;
                            }
                        }
                    }
                }

                customers.Add(new CustomerListViewModel
                {
                    CustomerId = c.CustomerId,
                    Name = c.Name,
                    Email = c.Email ?? "",
                    Phone = c.Phone ?? "",
                    Address = c.Address ?? "",
                    TotalMedicinesBought = totalMeds
                });
            }

            var medicinesList = await _context.Medicines
                .Include(m => m.Stocks)
                .Include(m => m.PurchaseItems)
                .ToListAsync();

            var salesStockData = new List<AdminSalesStockViewModel>();
            int totalStockQty = 0;

            foreach (var m in medicinesList)
            {
                int unitsSold = 0;
                if (m.PurchaseItems != null)
                {
                    foreach (var pi in m.PurchaseItems)
                    {
                        unitsSold += pi.Quantity;
                    }
                }

                int unitsLeft = 0;
                if (m.Stocks != null)
                {
                    foreach (var s in m.Stocks)
                    {
                        unitsLeft += s.Quantity;
                    }
                }

                totalStockQty += unitsLeft;

                decimal buyPrice = 0;
                decimal sellPrice = 0;
                if (m.Stocks != null && m.Stocks.Any())
                {
                    var latestStock = m.Stocks.OrderByDescending(s => s.CreatedAt).FirstOrDefault();
                    if (latestStock != null)
                    {
                        buyPrice = latestStock.PurchasePrice;
                        sellPrice = latestStock.SellingPrice;
                    }
                }

                salesStockData.Add(new AdminSalesStockViewModel
                {
                    MedicineId = m.MedicineId,
                    MedicineName = m.Name,
                    UnitsSold = unitsSold,
                    UnitsLeft = unitsLeft,
                    BuyPrice = buyPrice,
                    SellPrice = sellPrice
                });
            }

            var viewModel = new AdminDashboardViewModel
            {
                TotalCustomers = customers.Count,
                TotalMedicinesStock = totalStockQty,
                Customers = customers,
                SalesStocks = salesStockData
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.CustomerId == id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerPurchaseHistory(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .Include(c => c.Purchases)
                        .ThenInclude(p => p.PurchaseItems)
                            .ThenInclude(pi => pi.Medicine)
                    .FirstOrDefaultAsync(c => c.CustomerId == id);

                if (customer == null)
                {
                    return NotFound();
                }

                var purchaseOrders = new List<PurchaseOrderViewModel>();
                if (customer.Purchases != null)
                {
                    foreach (var p in customer.Purchases.OrderByDescending(x => x.PurchaseDate))
                    {
                        var items = new List<PurchaseItemViewModel>();
                        decimal totalAmt = 0;

                        if (p.PurchaseItems != null)
                        {
                            foreach (var pi in p.PurchaseItems)
                            {
                                var itemTotal = pi.Quantity * pi.UnitPrice;
                                totalAmt += itemTotal;

                                items.Add(new PurchaseItemViewModel
                                {
                                    MedicineName = pi.Medicine != null ? pi.Medicine.Name : "Unknown Medicine",
                                    Quantity = pi.Quantity,
                                    UnitPrice = pi.UnitPrice,
                                    TotalPrice = itemTotal
                                });
                            }
                        }

                        purchaseOrders.Add(new PurchaseOrderViewModel
                        {
                            PurchaseId = p.PurchaseId,
                            PurchaseDate = p.PurchaseDate,
                            TotalAmount = totalAmt,
                            PurchaseItems = items
                        });
                    }
                }

                var viewModel = new CustomerPurchaseHistoryAdminViewModel
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.Name ?? "Unknown Customer",
                    Purchases = purchaseOrders
                };

                return PartialView("~/Views/Admin/CustomerHistoryPartial.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // MEDICINE MANAGEMENT
        [HttpGet]
        public async Task<IActionResult> Medicines()
        {
            var medList = await _context.Medicines
                .Include(m => m.Category)
                .Include(m => m.Stocks)
                .ToListAsync();

            var medicines = new List<MedicineViewModel>();
            foreach (var m in medList)
            {
                int totalStock = 0;
                if (m.Stocks != null)
                {
                    foreach (var s in m.Stocks)
                    {
                        totalStock += s.Quantity;
                    }
                }

                decimal latestSellPrice = 0;
                if (m.Stocks != null && m.Stocks.Any())
                {
                    var stk = m.Stocks.OrderByDescending(s => s.CreatedAt).FirstOrDefault();
                    if (stk != null)
                    {
                        latestSellPrice = stk.SellingPrice;
                    }
                }

                medicines.Add(new MedicineViewModel
                {
                    MedicineId = m.MedicineId,
                    Name = m.Name,
                    GenericName = m.GenericName ?? "",
                    BrandName = m.BrandName ?? "",
                    Strength = m.Strength ?? "",
                    DosageForm = m.DosageForm ?? "",
                    CategoryName = m.Category != null ? m.Category.Name : "Uncategorized",
                    CategoryId = m.CategoryId,
                    Manufacturer = m.Manufacturer ?? "",
                    ImageUrl = m.ImageUrl ?? "",
                    IsPrescriptionRequired = m.IsPrescriptionRequired,
                    Description = m.Description ?? "",
                    IsActive = m.IsActive,
                    TotalStockQuantity = totalStock,
                    LatestSellingPrice = latestSellPrice
                });
            }

            return View("~/Views/Admin/Medicine/Index.cshtml", medicines);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            var viewModel = new MedicineCreateViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "Name")
            };
            return View("~/Views/Admin/Medicine/Create.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicineCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = string.Empty;
                if (viewModel.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/medicines");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    uniqueFileName = "/images/medicines/" + uniqueFileName;
                }

                var medicine = new PharmacyManagement.Models.Medicine
                {
                    Name = viewModel.Name,
                    GenericName = viewModel.GenericName,
                    CategoryId = viewModel.CategoryId,
                    Strength = viewModel.Strength,
                    ImageUrl = uniqueFileName,
                    IsActive = true
                };

                _context.Medicines.Add(medicine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Medicines));
            }

            var categories = await _context.Categories.ToListAsync();
            viewModel.Categories = new SelectList(categories, "CategoryId", "Name", viewModel.CategoryId);
            return View("~/Views/Admin/Medicine/Create.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.ToListAsync();
            var viewModel = new MedicineEditViewModel
            {
                MedicineId = medicine.MedicineId,
                Name = medicine.Name,
                CategoryId = medicine.CategoryId,
                ImageUrl = medicine.ImageUrl ?? "",
                Categories = new SelectList(categories, "CategoryId", "Name", medicine.CategoryId)
            };

            return View("~/Views/Admin/Medicine/Edit.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicineEditViewModel viewModel)
        {
            if (id != viewModel.MedicineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var medicine = await _context.Medicines.FindAsync(id);
                if (medicine == null)
                {
                    return NotFound();
                }

                if (viewModel.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/medicines");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ImageFile.CopyToAsync(fileStream);
                    }
                    medicine.ImageUrl = "/images/medicines/" + uniqueFileName;
                }

                medicine.Name = viewModel.Name;
                medicine.CategoryId = viewModel.CategoryId;

                _context.Update(medicine);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Medicines));
            }

            var categories = await _context.Categories.ToListAsync();
            viewModel.Categories = new SelectList(categories, "CategoryId", "Name", viewModel.CategoryId);
            return View("~/Views/Admin/Medicine/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MedicineId == id);

            if (medicine == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Medicine/Delete.cshtml", medicine);
        }

        [HttpPost, ActionName("DeleteMedicine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedicineConfirmed(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicines.Remove(medicine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Medicines));
        }

        // STOCK MANAGEMENT
        [HttpGet]
        public async Task<IActionResult> Stocks()
        {
            var stockList = await _context.Stocks
                .Include(s => s.Medicine)
                .ToListAsync();

            var stocks = new List<StockViewModel>();
            foreach (var s in stockList)
            {
                stocks.Add(new StockViewModel
                {
                    StockId = s.StockId,
                    MedicineId = s.MedicineId,
                    MedicineName = s.Medicine != null ? s.Medicine.Name : "Unknown",
                    BatchNumber = s.BatchNumber ?? "",
                    Quantity = s.Quantity,
                    PurchasePrice = s.PurchasePrice,
                    SellingPrice = s.SellingPrice,
                    ExpiryDate = s.ExpiryDate,
                    CreatedAt = s.CreatedAt
                });
            }

            return View("~/Views/Admin/Stock/Index.cshtml", stocks);
        }

        [HttpGet]
        public async Task<IActionResult> AddStock()
        {
            var meds = await _context.Medicines.ToListAsync();
            var viewModel = new StockCreateEditViewModel
            {
                Medicines = new SelectList(meds, "MedicineId", "Name")
            };
            return View("~/Views/Admin/Stock/AddStock.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(StockCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var stock = new PharmacyManagement.Models.Stock
                {
                    MedicineId = viewModel.MedicineId,
                    BatchNumber = viewModel.BatchNumber!,
                    Quantity = viewModel.Quantity,
                    PurchasePrice = viewModel.PurchasePrice,
                    SellingPrice = viewModel.SellingPrice,
                    ExpiryDate = viewModel.ExpiryDate,
                    CreatedAt = DateTime.Now
                };

                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Stocks));
            }

            var meds = await _context.Medicines.ToListAsync();
            viewModel.Medicines = new SelectList(meds, "MedicineId", "Name", viewModel.MedicineId);
            return View("~/Views/Admin/Stock/AddStock.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditStock(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            var meds = await _context.Medicines.ToListAsync();
            var viewModel = new StockCreateEditViewModel
            {
                StockId = stock.StockId,
                MedicineId = stock.MedicineId,
                BatchNumber = stock.BatchNumber ?? "",
                Quantity = stock.Quantity,
                PurchasePrice = stock.PurchasePrice,
                SellingPrice = stock.SellingPrice,
                ExpiryDate = stock.ExpiryDate,
                Medicines = new SelectList(meds, "MedicineId", "Name", stock.MedicineId)
            };

            return View("~/Views/Admin/Stock/Edit.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStock(int id, StockCreateEditViewModel viewModel)
        {
            if (id != viewModel.StockId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var stock = await _context.Stocks.FindAsync(id);
                if (stock == null)
                {
                    return NotFound();
                }

                stock.MedicineId = viewModel.MedicineId;
                stock.BatchNumber = viewModel.BatchNumber!;
                stock.Quantity = viewModel.Quantity;
                stock.PurchasePrice = viewModel.PurchasePrice;
                stock.SellingPrice = viewModel.SellingPrice;
                stock.ExpiryDate = viewModel.ExpiryDate;

                _context.Update(stock);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Stocks));
            }

            var meds = await _context.Medicines.ToListAsync();
            viewModel.Medicines = new SelectList(meds, "MedicineId", "Name", viewModel.MedicineId);
            return View("~/Views/Admin/Stock/Edit.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _context.Stocks
                .Include(s => s.Medicine)
                .FirstOrDefaultAsync(s => s.StockId == id);

            if (stock == null)
            {
                return NotFound();
            }

            return View("~/Views/Admin/Stock/Delete.cshtml", stock);
        }

        [HttpPost, ActionName("DeleteStock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStockConfirmed(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Stocks));
        }

        // PRESCRIPTION MANAGEMENT
        [HttpGet]
        public async Task<IActionResult> Prescriptions()
        {
            var presList = await _context.Prescriptions
                .Include(p => p.Customer)
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();

            var prescriptions = new List<PrescriptionViewModel>();
            foreach (var p in presList)
            {
                prescriptions.Add(new PrescriptionViewModel
                {
                    PrescriptionId = p.PrescriptionId,
                    CustomerId = p.CustomerId,
                    CustomerName = p.Customer != null ? p.Customer.Name : "Unknown",
                    FileUrl = p.FileUrl ?? "",
                    Notes = p.Notes ?? "",
                    Status = p.Status ?? "",
                    UploadedAt = p.UploadedAt
                });
            }

            return View(prescriptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(prescription.FileUrl))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, prescription.FileUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Prescriptions));
        }
    }
}
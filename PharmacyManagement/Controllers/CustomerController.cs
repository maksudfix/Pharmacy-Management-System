using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PharmacyManagement.Data;
using PharmacyManagement.Models;
using PharmacyManagement.Models.ViewModels.Cart;
using PharmacyManagement.Models.ViewModels.Prescription;
using PharmacyManagement.Models.ViewModels.Purchase;

namespace PharmacyManagement.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // CHECKOUT
        [HttpGet]
        public async Task<IActionResult> Checkout(int? medicineId, int? quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.CustomerId == null)
            {
                return Unauthorized();
            }

            var cartItems = new List<CartItemViewModel>();

            if (medicineId.HasValue == true)
            {
                if (quantity.HasValue == true)
                {
                    var medicine = await _context.Medicines
                        .Include(m => m.Stocks)
                        .FirstOrDefaultAsync(m => m.MedicineId == medicineId.Value);

                    if (medicine != null)
                    {
                        int totalStock = 0;
                        if (medicine.Stocks != null)
                        {
                            foreach (var st in medicine.Stocks)
                            {
                                totalStock += st.Quantity;
                            }
                        }

                        decimal unitPrice = 0.00m;
                        if (medicine.Stocks != null)
                        {
                            if (medicine.Stocks.Any() == true)
                            {
                                var latestStock = medicine.Stocks.OrderByDescending(s => s.CreatedAt).FirstOrDefault();
                                if (latestStock != null)
                                {
                                    unitPrice = latestStock.SellingPrice;
                                }
                            }
                        }

                        int initialQty = quantity.Value;
                        if (initialQty > totalStock)
                        {
                            if (totalStock > 0)
                            {
                                initialQty = totalStock;
                            }
                        }
                        if (initialQty < 1)
                        {
                            initialQty = 1;
                        }

                        var singleCartItem = new CartItemViewModel();
                        singleCartItem.MedicineId = medicine.MedicineId;
                        singleCartItem.MedicineName = medicine.Name ?? "Unknown";
                        singleCartItem.UnitPrice = unitPrice;
                        singleCartItem.Quantity = initialQty;
                        singleCartItem.StockLimit = totalStock;

                        cartItems.Add(singleCartItem);
                    }
                }
            }

            var rawPrescriptions = await _context.Prescriptions
                .Where(p => p.CustomerId == user.CustomerId.Value)
                .ToListAsync();

            var prescriptions = new List<SelectListItem>();
            foreach (var p in rawPrescriptions)
            {
                var listItem = new SelectListItem();
                listItem.Value = p.PrescriptionId.ToString();
                listItem.Text = "Prescription #" + p.PrescriptionId + " - Uploaded: " + p.UploadedAt.ToString("yyyy-MM-dd") + " (" + p.Status + ")";
                prescriptions.Add(listItem);
            }

            var viewModel = new CheckoutViewModel();
            viewModel.CustomerId = user.CustomerId.Value;
            viewModel.CartItems = cartItems;
            viewModel.PaymentMethod = "CashOnDelivery";
            viewModel.DeliveryMethod = "HomeDelivery";
            viewModel.Prescriptions = prescriptions;

            return View("~/Views/Customer/Checkout.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCheckout(CheckoutViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CustomerId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid == true)
            {
                decimal totalAmount = 0;
                foreach (var item in viewModel.CartItems)
                {
                    totalAmount += (item.UnitPrice * item.Quantity);
                }

                Purchase purchase = new Purchase();
                purchase.CustomerId = user.CustomerId.Value;
                purchase.TotalAmount = totalAmount;
                purchase.PaymentMethod = viewModel.PaymentMethod;
                purchase.Status = "Pending";
                purchase.PurchaseDate = DateTime.Now;

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyPurchases));
            }

            var rawPrescriptions = await _context.Prescriptions
                .Where(p => p.CustomerId == user.CustomerId.Value)
                .ToListAsync();

            var prescriptions = new List<SelectListItem>();
            foreach (var p in rawPrescriptions)
            {
                var listItem = new SelectListItem();
                listItem.Value = p.PrescriptionId.ToString();
                listItem.Text = "Prescription #" + p.PrescriptionId + " - Uploaded: " + p.UploadedAt.ToString("yyyy-MM-dd") + " (" + p.Status + ")";
                prescriptions.Add(listItem);
            }

            viewModel.Prescriptions = prescriptions;

            return View("~/Views/Customer/Checkout.cshtml", viewModel);
        }

        // PURCHASES
        [HttpGet]
        public async Task<IActionResult> MyPurchases()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CustomerId == null)
            {
                return Unauthorized();
            }

            var rawPurchases = await _context.Purchases
                .Where(p => p.CustomerId == user.CustomerId.Value)
                .ToListAsync();

            var purchases = new List<PurchaseViewModel>();
            foreach (var p in rawPurchases)
            {
                PurchaseViewModel pvm = new PurchaseViewModel();
                pvm.PurchaseId = p.PurchaseId;
                pvm.TotalAmount = p.TotalAmount;
                pvm.PaymentMethod = p.PaymentMethod;
                pvm.Status = p.Status;
                pvm.PurchaseDate = p.PurchaseDate;
                purchases.Add(pvm);
            }

            return View("~/Views/Customer/MyPurchases.cshtml", purchases);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int purchaseId)
        {
            var purchase = await _context.Purchases.FindAsync(purchaseId);
            if (purchase != null)
            {
                purchase.Status = "Completed";
                _context.Update(purchase);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyPurchases));
        }

        // PRESCRIPTIONS
        [HttpGet]
        public async Task<IActionResult> MyPrescriptions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CustomerId == null)
            {
                return Unauthorized();
            }

            var rawPrescs = await _context.Prescriptions
                .Where(p => p.CustomerId == user.CustomerId.Value)
                .ToListAsync();

            var prescriptions = new List<PrescriptionViewModel>();
            foreach (var p in rawPrescs)
            {
                PrescriptionViewModel pvm = new PrescriptionViewModel();
                pvm.PrescriptionId = p.PrescriptionId;
                pvm.CustomerId = p.CustomerId;
                pvm.FileUrl = p.FileUrl;
                pvm.Notes = p.Notes;
                pvm.Status = p.Status;
                pvm.UploadedAt = p.UploadedAt;
                prescriptions.Add(pvm);
            }

            return View("~/Views/Customer/MyPrescriptions.cshtml", prescriptions);
        }

        [HttpGet]
        public IActionResult UploadPrescription()
        {
            return View("~/Views/Customer/UploadPrescription.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPrescription(PrescriptionUploadViewModel viewModel)
        {
            if (ModelState.IsValid == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null || user.CustomerId == null)
                {
                    return Unauthorized();
                }

                string uniqueFileName = string.Empty;
                if (viewModel.PrescriptionFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/prescriptions");
                    if (Directory.Exists(uploadsFolder) == false)
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.PrescriptionFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.PrescriptionFile.CopyToAsync(fileStream);
                    }
                    uniqueFileName = "/uploads/prescriptions/" + uniqueFileName;
                }

                // Automatically set status to "Done" upon upload as requested
                Prescription prescription = new Prescription();
                prescription.CustomerId = user.CustomerId.Value;
                prescription.FileUrl = uniqueFileName;
                prescription.Notes = viewModel.Notes;
                prescription.Status = "Done";
                prescription.UploadedAt = DateTime.Now;

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyPrescriptions));
            }

            return View("~/Views/Customer/UploadPrescription.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.CustomerId == null)
            {
                return Unauthorized();
            }

            var prescription = await _context.Prescriptions
                .FirstOrDefaultAsync(p => p.PrescriptionId == id && p.CustomerId == user.CustomerId.Value);

            if (prescription == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(prescription.FileUrl) == false)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, prescription.FileUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath) == true)
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyPrescriptions));
        }
    }
}
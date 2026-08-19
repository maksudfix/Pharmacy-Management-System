# Pharmacy Management System

> A full-stack, responsive Pharmacy Management System built with ASP.NET Core MVC, Entity Framework Core, and SQL Server. The application provides authentication, role-based access control, medicine and category management, stock management, prescription handling, purchasing, shopping cart, checkout, customer management, and an administrative dashboard.

## Features

* **Authentication & Authorization**

  * ASP.NET Core Identity integration
  * Role-based access control with `Admin` and `Customer` roles
  * Login and registration
  * Access-denied handling

* **Role-Based Access Control**

  * Administrative access for pharmacy management
  * Protected routes and authorization policies
  * Separate customer and admin functionality

* **Medicine Management**

  * Create, read, update, and delete medicines
  * Medicine details and information management
  * Category-based organization

* **Category Management**

  * Create and manage medicine categories
  * Organize medicines for easier browsing

* **Stock Management**

  * Monitor medicine stock
  * Add and update stock information
  * Track available quantities
  * Admin sales and stock overview

* **Prescription Management**

  * Customers can upload prescriptions
  * Admin can review uploaded prescriptions
  * Prescription records and management

* **Shopping Cart & Checkout**

  * Add medicines to cart
  * Update and remove cart items
  * Checkout functionality
  * Purchase and purchase-item management

* **Customer Management**

  * Customer registration and account management
  * Customer purchase history
  * Personal prescription and purchase information

* **Admin Dashboard**

  * Centralized pharmacy management dashboard
  * Medicine and stock overview
  * Customer purchase history
  * Prescription management
  * Sales and inventory information

* **ViewModel-Based Architecture**

  * Dedicated ViewModels for forms and UI operations
  * Separates user input from application entities
  * Supports cleaner model binding and validation

* **Responsive UI**

  * Razor Views with Bootstrap
  * Custom CSS and JavaScript
  * Responsive layouts for different screen sizes

## Architecture

Pharmacy Management System follows the **Model-View-Controller (MVC)** architectural pattern and uses ViewModels to separate user input from application entities.
![Pharmacy Management System Dashboard](images/pharmacy-dashboard.png) <img width="1408" height="768" alt="Gemini_Generated_Image_6balia6balia6bal" src="https://github.com/user-attachments/assets/4a51e36c-303b-47d5-b949-2e3957316f25" />


### Architecture Overview

**Controllers**
Handle HTTP requests, application flow, authorization, and communication between Views, ViewModels, Identity, and the database.
**ViewModels**
Provide dedicated models for operations such as customer registration, login, medicine creation/editing, prescription uploading, purchases, cart operations, and stock management.

**Entity Framework Core**
Provides database access through `AppDbContext` and manages the application's data model and migrations.

**ASP.NET Core Identity**
Handles authentication, authorization, roles, password management, and account-related functionality.

**Razor Views**
Provide the presentation layer using Razor, HTML, CSS, JavaScript, Bootstrap, and custom UI components.

## Project Structure
PharmacyManagement/
├── Controllers/
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── CustomerController.cs
│   └── HomeController.cs
│
├── Data/
│   ├── AppDbContext.cs
│   └── Migrations/
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── Category.cs
│   ├── Customer.cs
│   ├── Medicine.cs
│   ├── Prescription.cs
│   ├── Purchase.cs
│   ├── PurchaseItem.cs
│   └── Stock.cs
│
├── ViewModels/
│   ├── Admin/
│   │   ├── AdminDashboardViewModel.cs
│   │   └── CustomerPurchaseHistoryAdminViewModel.cs
│   │
│   ├── Cart/
│   │   ├── CartItemViewModel.cs
│   │   └── CheckoutViewModel.cs
│   │
│   ├── Category/
│   │   ├── CategoryCreateEditViewModel.cs
│   │   └── CategoryViewModel.cs
│   │
│   ├── Customer/
│   │   ├── CustomerCreateEditViewModel.cs
│   │   ├── CustomerLoginViewModel.cs
│   │   ├── CustomerRegisterViewModel.cs
│   │   └── CustomerViewModel.cs
│   │
│   ├── Medicine/
│   │   ├── MedicineCreateViewModel.cs
│   │   ├── MedicineEditViewModel.cs
│   │   └── MedicineViewModel.cs
│   │
│   ├── Prescription/
│   │   ├── PrescriptionCreateEditViewModel.cs
│   │   ├── PrescriptionUploadViewModel.cs
│   │   └── PrescriptionViewModel.cs
│   │
│   ├── Purchase/
│   │   ├── PurchaseItemViewModel.cs
│   │   └── PurchaseViewModel.cs
│   │
│   └── Stock/
│       ├── AdminSalesStockViewModel.cs
│       ├── StockCreateEditViewModel.cs
│       └── StockViewModel.cs
│
├── Views/
│   ├── Admin/
│   │   ├── Medicine/
│   │   ├── Stock/
│   │   ├── CustomerHistoryPartial.cshtml
│   │   ├── Dashboard.cshtml
│   │   └── Prescriptions.cshtml
│   │
│   ├── Auth/
│   │   ├── AccessDenied.cshtml
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   │
│   ├── Customer/
│   │   ├── Checkout.cshtml
│   │   ├── MyPrescriptions.cshtml
│   │   ├── MyPurchases.cshtml
│   │   └── UploadPrescription.cshtml
│   │
│   ├── Home/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── images/
│   ├── js/
│   ├── lib/
│   └── uploads/
│
├── appsettings.json
├── Program.cs
└── PharmacyManagement.csproj

## Tech Stack

| Technology                  | Purpose                                  |
| --------------------------- | ---------------------------------------- |
| **ASP.NET Core MVC**        | Web application framework                |
| **C#**                      | Application programming language         |
| **Entity Framework Core**   | ORM and database access                  |
| **SQL Server**              | Relational database                      |
| **ASP.NET Core Identity**   | Authentication and authorization         |
| **Razor Views**             | Server-side UI rendering                 |
| **Bootstrap / Bootswatch**  | Responsive UI components and styling     |
| **HTML / CSS / JavaScript** | Frontend development                     |
| **jQuery**                  | Client-side functionality and validation |

## Prerequisites

Make sure the following are installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* SQL Server, SQL Server Express, or SQL Server LocalDB
* Visual Studio 2026 or VS Code with the C# Dev Kit
* Entity Framework Core CLI (`dotnet-ef`)

> **Target Framework:** `.NET 10`
>
> **ASP.NET Core Identity:** `10.0.0`
>
> **Entity Framework Core:** `10.0.0`

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/maksudfix/Pharmacy-Management-System.git
cd Pharmacy-Management-System
```

### 2. Configure the Database

Update the connection string in `appsettings.json` according to your SQL Server environment.
,
  "ConnectionStrings": {
    "DefaultConnection": "Server=HP-TREX\\SQLEXPRESS; Database = pharmacy_db; Trusted_Connection = True; TrustServerCertificate = True;"
  }

> For production environments, use environment variables, User Secrets, Azure Key Vault, or another secure configuration provider rather than committing sensitive connection information to source control.

### 3. Apply Entity Framework Migrations

To create a new migration:

```powershell
Add-Migration Initial
```

To apply the migration:

```powershell
Update-Database
```

Or using the .NET CLI:

```bash
dotnet ef database update
```

### 4. Run the Application
dotnet run


## Project Goals

Pharmacy Management System was built to demonstrate practical experience with:

* ASP.NET Core MVC application development
* C# and object-oriented programming
* Entity Framework Core
* SQL Server database integration
* ASP.NET Core Identity
* Authentication and authorization
* Role-based access control
* CRUD operations
* Medicine and category management
* Stock and inventory management
* Prescription management
* Shopping cart and checkout functionality
* Purchase and sales management
* ViewModels and model binding
* Razor Views
* Database migrations
* File upload functionality
* Responsive web development

## Future Improvements

* **AI-Based Prescription Analysis**
  * Analyze uploaded prescriptions using AI-assisted document analysis
  * Extract medicine names and prescription information
  * Provide medicine suggestions based on the analyzed prescription
  * Add appropriate validation and pharmacist/admin review before any recommendation is acted upon

* **Customer Message Box**
  * Add a messaging system between customers and pharmacy administrators
  * Allow customers to ask questions about medicines, prescriptions, orders, and purchases
  * Provide an admin interface for managing customer conversations

* **Architecture & Clean Code**
  * Refactor controllers to follow SOLID principles
  * Add centralized exception handling
  * Implement structured logging
  * Improve service-layer architecture

* **Performance & Scale**
  * Implement caching
  * Optimize database queries
  * Add pagination for large datasets
  * Improve inventory and search performance

## Author
Maksud Mubin (Trex Development)

GitHub: https://github.com/maksudfix

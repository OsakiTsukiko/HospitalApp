# Hospital Management System

A comprehensive web-based Hospital Management System built with ASP.NET Core MVC, Entity Framework Core, and SQL Server. This system manages three types of users: Patients, Doctors, and Administrators.

## Features

### 🔐 Authentication & Authorization
- User registration with role-based signup (Patient, Doctor, Admin)
- Secure login/logout functionality
- Role-based access control

### 👥 User Management
- **Patients**: Complete medical profile management including blood type, allergies, emergency contacts, and medical history
- **Doctors**: Professional profile with department, experience, and license information
- **Administrators**: Full system access with user management capabilities

### 📊 Dashboard Features
- **Patient Dashboard**: Personal medical information display
- **Doctor Dashboard**: Professional profile overview
- **Admin Dashboard**: Complete user management with statistics and user deletion capabilities

### ✏️ Profile Management
- Update personal information
- Role-specific field management
- Secure data validation

## Technology Stack

- **Framework**: ASP.NET Core 9.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5, HTML5, CSS3, JavaScript
- **ORM**: Entity Framework Core 9.0

## Database Schema

### User (Base Class)
- Email (Primary identifier)
- Password (Hashed)
- Role (Patient, Doctor, Admin)
- Name
- CNP (Personal Identification Number)
- Phone Number

### Patient (Inherits from User)
- Blood Type
- Emergency Contact
- Allergies
- Weight
- Height
- Birth Date
- Address

### Doctor (Inherits from User)
- Department
- Years of Experience
- License Number

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd HospitalManagement
```

### 2. Database Configuration
The application is configured to use SQL Server with the following connection string:
```
Server=localhost\SQLExpress;Database=HMS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

### 3. Install Dependencies
```bash
dotnet restore
```

### 4. Database Migration
```bash
# Create migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### 5. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Usage

### First Time Setup
1. Navigate to the application URL
2. Click "Register" to create your first user account
3. Select your role (Patient, Doctor, or Admin)
4. Fill in the required information based on your role
5. Complete registration and login

### User Roles & Permissions

#### 👤 Patient
- View personal medical dashboard
- Update medical information (blood type, allergies, emergency contacts, etc.)
- Manage personal profile

#### 👨‍⚕️ Doctor
- View professional dashboard
- Update professional information (department, experience, license)
- Manage personal profile

#### 👨‍💼 Administrator
- Access admin dashboard with user statistics
- View all users in the system
- Delete patients and doctors (cannot delete other admins)
- Manage system users

### Key Features

#### Registration Process
- Dynamic form fields based on selected user role
- Client-side validation
- Secure password requirements
- Role-specific data collection

#### Dashboard Views
- Role-specific information display
- Clean, professional interface
- Easy navigation between features

#### Profile Management
- Update personal information
- Role-specific field editing
- Data validation and error handling

## Project Structure

```
HospitalManagement/
├── Controllers/
│   ├── AccountController.cs      # Authentication & Registration
│   └── HomeController.cs         # Dashboard & Profile Management
├── Models/
│   ├── User.cs                   # Base user model
│   ├── Patient.cs                # Patient-specific model
│   ├── Doctor.cs                 # Doctor-specific model
│   ├── HospitalDbContext.cs      # Database context
│   └── ViewModels/
│       ├── LoginViewModel.cs     # Login form model
│       └── RegisterViewModel.cs  # Registration form model
├── Views/
│   ├── Account/
│   │   ├── Login.cshtml          # Login page
│   │   └── Register.cshtml       # Registration page
│   ├── Home/
│   │   ├── Dashboard.cshtml      # User dashboard
│   │   ├── AdminDashboard.cshtml # Admin dashboard
│   │   └── UpdateProfile.cshtml  # Profile update form
│   └── Shared/
│       └── _Layout.cshtml        # Main layout template
├── appsettings.json              # Configuration
├── Program.cs                    # Application startup
└── HospitalManagement.csproj     # Project file
```

## Security Features

- Password hashing using ASP.NET Core Identity
- Role-based authorization
- CSRF protection
- SQL injection prevention through Entity Framework
- Secure session management

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support or questions, please create an issue in the repository.

---

**Note**: This is a demonstration project for educational purposes. For production use, additional security measures, testing, and features should be implemented. 

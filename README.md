
# Attendance Tracker Web Application



The Attendance Tracker Web Application is a tool that allows users to record and manage attendance records. It provides features for users to clock in and out, view their attendance history, and perform administrative tasks if they have the necessary permissions.

## Features

- User Registration and Login: Users can register for an account and log in to the application.
- Clock In and Clock Out: Logged-in users can clock in and clock out, recording their attendance times.
- View Attendance History: Users can view their own attendance history, including clock-in and clock-out times.
- Admin Dashboard: Admin users have access to an admin dashboard to manage users and attendance records.
- Role-Based Access Control: Different roles (e.g., User and Admin) have different levels of access to features.

## Installation and Setup

1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/attendance-tracker.git
   cd attendance-tracker
## Install dependencies:
    dotnet restore
## Configure the database:

Modify the connection string in appsettings.json to point to your database.
Run migrations:
  dotnet ef database update


# Usage
- Register an account or log in if you already have one.
- Clock in and out using the provided buttons.
- View your attendance history on the index page.
- Admin users can access the admin dashboard to manage users and records.



# Technologies Used
- ASP.NET Core MVC
- Entity Framework Core
- Identity Framework
- HTML, CSS, JavaScript


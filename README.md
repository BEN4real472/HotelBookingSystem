# Hotel Booking System

This project is a web-based Hotel Booking System developed using ASP.NET Core MVC and Entity Framework Core. The system allows users to create, edit, and manage hotel bookings while ensuring data validation and preventing logical errors.

## Features

- Create, edit, and delete bookings
- Validation to prevent invalid dates (check-in cannot be in the past, check-out must be after check-in)
- Prevention of overlapping bookings for the same room
- Automatic calculation of number of nights
- Automatic calculation of total booking price
- Dropdown selection for users and rooms to reduce input errors

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- C#

## How to Run the Project

1. Open the project in Visual Studio
2. Restore NuGet packages if required
3. Run the application using IIS Express
4. Navigate to the "Bookings" page to manage bookings

## Project Structure

- Models: Defines system entities (Booking, Room, User)
- Controllers: Handles application logic (BookingsController)
- Views: User interface for booking operations
- Database: Managed using Entity Framework Core

## Author

Developed as part of coursework for Software Engineering (Level 6).

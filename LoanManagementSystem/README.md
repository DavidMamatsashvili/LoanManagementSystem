# Loan Management System

## Overview

Loan Management System is an ASP.NET Core Web API application designed to manage customers, loan applications, payments, loan schedules, and administrative operations. The system supports authentication and authorization using JWT Bearer tokens and provides role-based access control for Customers and Administrators.

## Features

### Customer Features

* Customer registration
* Customer login with JWT authentication
* View personal loan information
* Apply for new loans
* View loan schedules
* Make loan payments
* View payment history

### Loan Management

* Create loan applications
* Automatic monthly payment (PMT) calculation
* Loan approval or rejection based on business rules
* Loan status tracking:

  * Pending
  * Approved
  * Rejected
  * Closed
  * Overdue
* Generate loan repayment schedules

### Payment Management

* Make payments towards active loans
* Prevent payments on closed loans
* Prevent overpayments
* Update loan balance after payments
* Automatically close loans when fully paid

### Credit Score Management

* Increase credit score for timely and sufficient payments
* Decrease credit score for late or insufficient payments

### Administrative Features

* Administrator authentication
* View all customers
* View all loans
* Delete customers
* Delete loans

## Technologies Used

* ASP.NET Core Web API (.NET 8)
* Entity Framework Core
* SQL Server
* JWT Authentication
* Role-Based Authorization
* FluentValidation
* Swagger / OpenAPI

## Project Structure

```
LoanManagementSystem
├── Controllers
├── CustomDataValidators
├── DTOs
├── Extensions
├── Migrations
├── Models
├── Repository
└── Services
```

## Domain Models

### Customer

* Id
* Email
* PasswordHash
* FirstName
* LastName
* PersonalNumber
* BirthDate
* CreditScore

### Loan

* Id
* CustomerId
* Amount
* InterestRate
* TermMonths
* MonthlyPayment
* Status
* CreatedAt

### Payment

* Id
* LoanId
* Amount
* PaymentDate

### LoanSchedule

* Id
* LoanId
* PMT
* Date

### Admin

* Id
* Email
* PasswordHash
* Role

## Authentication and Authorization

The application uses JWT Bearer authentication.

### Roles

* Customer
* Admin

Example authorization:

```csharp
[Authorize(Roles = nameof(Roles.Admin))]
```

## API Endpoints

### Customer Endpoints

| Method | Endpoint                             | Description                          |
| ------ | ------------------------------------ | ------------------------------------ |
| POST   | `/api/Customer/RegisterCustomer`     | Register a new customer              |
| POST   | `/api/Customer/LoginCustomer`        | Customer login                       |
| GET    | `/api/Customer/GetCustomerById/{id}` | Get customer by ID                   |
| GET    | `/api/Customer/loans`                | Get loans for authenticated customer |

### Loan Endpoints

| Method | Endpoint                                   | Description                                                  |
| ------ | ------------------------------------------ | ------------------------------------------------------------ |
| POST   | `/api/Loan/CreateApplication`              | Create a new loan application for the authenticated customer |
| GET    | `/api/Loan/{id}`                           | Retrieve loan details by loan ID                             |
| GET    | `/api/Loan/GetLoanByCustomerId`            | Retrieve all loans belonging to the authenticated customer   |
| GET    | `/api/Loan/GetLoanScheduleByLoanId/{id}`   | Retrieve the repayment schedule for a specific loan          |
| GET    | `/api/Loan/GetLoanStatusByLoanId/{loanId}` | Retrieve the current status of a specific loan               |

### Payment Endpoints

| Method | Endpoint                                | Description             |
| ------ | --------------------------------------- | ----------------------- |
| POST   | `/api/Payment/Payments`                 | Make payment            |
| GET    | `/api/Payment/GetPaymentsByLoanId/{id}` | Get payments by loan ID |

### Admin Endpoints

| Method | Endpoint                              | Description         |
| ------ | ------------------------------------- | ------------------- |
| POST   | `/api/Admin/Login`                    | Administrator login |
| GET    | `/api/Admin/GetCustomers`             | Get all customers   |
| GET    | `/api/Admin/GetLoans`                 | Get all loans       |
| DELETE | `/api/Admin/DeleteCustomerById/{id}`  | Delete customer     |
| DELETE | `/api/Admin/DeleteLoanById/{loanId}`  | Delete loan         |

## Business Rules

### Loan Application Rules

* Customer must be at least 18 years old
* Loan amount must be greater than zero
* Loan term must be valid
* Credit score requirements may apply

### Payment Rules

* Loan must exist
* Customer must own the loan
* Closed loans cannot receive payments
* Payment amount must be greater than zero
* Payment amount cannot exceed the remaining balance

### Credit Score Rules

* Timely and sufficient payments increase credit score
* Late or insufficient payments decrease credit score

## Status Codes

### Success Responses

* `200 OK`
* `201 Created`
* `204 No Content`

### Error Responses

* `400 Bad Request`
* `401 Unauthorized`
* `404 Not Found`

## Running the Application

### Prerequisites

* .NET 8 SDK
* SQL Server
* Visual Studio 2022 or VS Code

### Database Setup

Update the connection string in `appsettings.json`.

Run migrations:

```bash
dotnet ef database update
```

### Running the API

```bash
dotnet run
```

Swagger UI will be available at:

```
https://localhost:<port>/swagger
```

## Future Improvements

* Refresh token implementation
* Soft delete functionality
* Global exception handling middleware
* AutoMapper integration
* Unit and integration testing
* Logging with Serilog
* Docker support
* Pagination and filtering
* Email notifications




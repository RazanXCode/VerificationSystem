# Document Verification System

## Overview
The Document Verification System enables users to upload documents, generate a unique verification code for each document, and later check whether the document is verified using the verification code. The system is built using ASP.NET Core for the backend and Angular for the frontend, providing a seamless and efficient process for document management and verification.

## Features
- **View Dashboard:** Users can access a dashboard that displays all the details of their documents.
- **Upload Documents:** Users can upload files (PDF, DOC, DOCX) along with a name, which are then stored in the database with a unique verification code.
- **Document Verification:** Users can check whether the admin has verified their document by submitting the unique verification code.


## Technologies Used
- **Backend:** ASP.NET Core, Entity Framework Core, Dapper
- **Frontend:** Angular
- **Database:** SQL Server


---

## Setup Instructions

### Prerequisites

- .NET 6 or later
- Node.js and npm
- SQL Server (or any compatible database)
- Angular CLI (for running the frontend)

### Backend Setup
1. Clone the repository:
   git clone https://github.com/yourusername/document-verification-system.git
   cd document-verification-system

2. Restore NuGet packages:
   dotnet restore

  
3. Configure your database connection in the appsettings.json file:
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=DocumentVerificationDb;User Id=your_user;Password=your_password;"
}



4.  Apply database migrations and seed the database:
  "ConnectionStrings": {
      "DefaultConnection": "Server=your_server;Database=DocumentVerificationDb;User Id=your_user;Password=your_password;"
  }

### Frontend Setup

1. Navigate to the `Frontend` folder: 
   cd Frontend

2. Start the development server:
   ng serve







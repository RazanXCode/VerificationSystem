# Document Verification System

## Overview
The Document Verification System allows users to easily upload documents, each associated with a unique verification code, and manage them on a dashboard. Admins can verify the uploaded documents using the verification code, and the document status is updated dynamically. Users can see the verification status of their documents in real-time.

## Project Features
**1. View Dashboard**

Users can access a dashboard that displays all details of their documents, including the name, upload status, and verification status.

**2. Upload Documents**

Users can upload their documents (PDF, DOC, DOCX formats) through a simple form.
Each document requires a title and must be associated with a file.
Upon successful upload, the document is stored in the database along with a unique verification code, which will be used for the verification process.

**3. Document Verification**

Admins have the ability to verify documents by providing the Document ID and Verification Code.
Users can check the verification status of their documents in the dashboard.

---

## Technologies Used
- **Backend:** ASP.NET Core, Entity Framework Core, Dapper
- **Frontend:** Angular
- **Database:** SQL Server


## Setup Instructions

### Prerequisites

- .NET 6 or later
- Node.js and npm
- SQL Server (or any compatible database)
- Angular CLI (for running the frontend)

### Backend Setup
1. Clone the repository:
   
   `git clone https://github.com/yourusername/document-verification-system.git
   cd document-verification-system`

3. Restore NuGet packages:
   
   `dotnet restore`

  
5. Configure your database connection in the appsettings.json file:
   
`"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=DocumentVerificationDb;User Id=your_user;Password=your_password;"
}`



7.  Apply database migrations and seed the database:
   
  `dotnet ef migrations add InitialCreate
dotnet ef migrations add InitialCreate
`

### Frontend Setup

1. Navigate to the `Frontend` folder, then
`cd Frontend`
   

2. Start the development server:
   
   `ng serve`




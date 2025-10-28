# 📰 FU News Management System

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?logo=microsoftsqlserver)
![Architecture](https://img.shields.io/badge/Clean%20Architecture-3%20Layers-success)
![Status](https://img.shields.io/badge/Status-Active-brightgreen)
![License](https://img.shields.io/badge/License-None-lightgrey)

A web-based system designed to manage and publish university news.  
It allows staff members to create, edit, duplicate, and organize news articles, while administrators manage system accounts and overall platform control.

---

## 🚀 Overview

The **FU News Management System** is an internal tool for **FPT University**, providing a centralized platform for managing and delivering news to lecturers and the public.

The system includes:
- A **public-facing page** for viewing news
- **Staff tools** for content management
- **Admin control** for user and system management
- **Secure authentication** and **clean architecture** implementation

---

## 🧩 Main Features

### 🧑‍🏫 Public
- View public news
- Update personal profile (name, password)

### 👩‍💼 Staff
- **CRUD News**
- **Duplicate existing news**
- **CRUD Tag**
- **CRUD Category**

### 👨‍💻 Admin
- Manage system accounts (create, update, delete, enable/disable users)

---

## 🧠 Technical Stack

| Layer | Technology |
|-------|-------------|
| Frontend | ASP.NET Razor Pages |
| Backend | ASP.NET Web API (.NET) |
| Database | Microsoft SQL Server |
| Authentication | JWT (JSON Web Token) |
| API Features | CORS configuration, OData support |
| Architecture | CLEAN architecture — 3 layers (Interface, Implementation, Presentation) |

---

## ⚙️ Setup & Installation

### 1️⃣ Database Setup
1. Open **SQL Server Management Studio**
2. Run the provided `.sql` script to initialize the database

### 2️⃣ Backend Setup
1. Open the `_BE` solution in **Visual Studio**
2. Configure the `appsettings.json` connection string to match your SQL Server
3. Run the backend project — the Web API will start serving on the configured port

### 3️⃣ Frontend Setup
1. Open the `_FE` solution in **Visual Studio**
2. Ensure backend URL is correctly configured in the frontend service or environment settings
3. Run the Razor Page project to start the client interface

---

## 🏗️ Project Architecture

The backend follows a **Clean Architecture** with three main layers:

Presentation Layer (API)
└── Application Layer (Interfaces)
└── Infrastructure Layer (Implementations)


✅ Separation of concerns  
✅ Easier maintainability and testing  
✅ Scalable for future modules (e.g., analytics, notifications)

---

## 🔐 Authentication & Security

- Implemented **JWT-based Authentication**
- Configured **CORS policy** to allow cross-origin requests from the frontend
- Uses **role-based authorization** (Admin, Staff, Public)

---

## 📁 Folder Structure (Backend Example)

_BE/
├── FUNMS.API/ # Web API (Presentation Layer)
├── FUNMS.BLL/ # Business Logic Layer
├── FUNMS.DAL/ # Data Access Layer
└── FUNMS.Shared/ # Shared Package


---

## 👤 Author

**Đỗ Hoàng Anh**  
📧 _aka_ **Anhdhhe181538**  
🎓 FPT University  

---

## 📜 License

This project is **not licensed**.  
All rights reserved © 2025 Đỗ Hoàng Anh.

---

## 🌐 Notes

- Ensure SQL Server service is running before launching the application.


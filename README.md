# 🚌 Bus Booking System API

A robust and scalable RESTful API for managing intercity bus reservations, built with **.NET 8** and **Clean Architecture** principles. This project demonstrates advanced backend concepts such as concurrency handling, background jobs, and complex business validation rules.

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Postgres](https://img.shields.io/badge/postgres-%23316192.svg?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

## 📖 Table of Contents
- [Project Overview](#-project-overview)
- [Architecture](#-architecture)
- [Technologies & Tools](#-technologies--tools)
- [Key Features](#-key-features)
- [Getting Started](#-getting-started)
- [Database Structure](#-database-structure)
- [Team & Workflow](#-team--workflow)

## 🚀 Project Overview
This system is designed to handle the core operations of a bus ticketing platform. It solves real-world problems like:
- preventing double-booking (Race Condition),
- managing gender-based seating restrictions,
- and automating reservation cancellations for unpaid tickets.

## 🏗 Architecture
The solution follows the **Clean Architecture (Onion Architecture)** pattern to ensure separation of concerns and testability.


BusBookingSystem.sln
├── 1. Core (Domain)           # Entities, Enums, Interfaces (No dependencies)
├── 2. Infrastructure          # EF Core, Database Context, Migrations
├── 3. Application             # Business Logic, DTOs, Services, Validators
└── 4. API (Presentation)      # Controllers, Middleware, Entry Point

## 🤝 Ekip Çalışması ve Git Kuralları


### 1. Dallanma Kuralları (Branching)

* **`main`:** Canlı (Production) kod. Buraya doğrudan `push` **YASAKTIR**.
* **`develop`:** Ana geliştirme ve entegrasyon dalıdır. Tüm PR'lar bu dalı hedefler.
* **`feature/[görev-adı]`:** Bireysel görev ve özellik geliştirme dallarıdır.

### 2. Günlük İş Akışı

Yeni bir göreve başlarken ve bitirirken şu adımları izleyin:

1.  **Güncel Kodu Çek:** Her zaman `develop` dalından başlayın ve güncelleyin:
    ```bash
    git checkout develop
    git pull origin develop
    ```
2.  **Yeni Dal Aç:** Görevinize özel bir dal açın:
    ```bash
    git checkout -b feature/otobus-ekleme
    ```
3.  **Kaydet (Commit):** Çalışmalarınızı düzenli olarak kaydedin:
    ```bash
    git add .
    git commit -m "feat: [Yapılan işin özeti]"
    ```
4.  **Paylaş (Push):** Kodu kendi dalınıza yükleyin:
    ```bash
    git push -u origin feature/otobus-ekleme
    ```
5.  **Birleştirme (PR):** İşiniz bittiğinde GitHub üzerinden **`develop`** dalına **Pull Request** açın.

### 3. Veritabanı (Migration) Kuralları ⚠️

Veritabanı şeması değişikliklerinde (Migration), çakışmayı önlemek için disiplin şarttır:

* **Migration Oluşturma:** `dotnet ef migrations add [İsim]` komutu **YALNIZCA DB Master** tarafından çalıştırılır.
* **Senkronizasyon:** DB Master `develop`'a birleştirme yaptıktan sonra, tüm ekip üyeleri çalışmaya devam etmeden önce mutlaka `git pull` yapmalıdır.

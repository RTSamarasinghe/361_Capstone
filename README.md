#  E-Commerce Application 361-Capstone Project

## Overview

This project is a full-stack e-commerce application built with a **React frontend** and a **.NET backend**, following structured design principles inspired by iDesign.

The application allows users to browse products, manage a cart, and complete purchases while supporting backend modularity through Managers and Engines.

---

## Tech Stack

### Frontend

* React
* JavaScript / TypeScript (optional)
* REST API integration

### Backend

* .NET
* C#
* iDesign-inspired architecture
* RESTful APIs

---

## Frontend Features

### Pages

####  Login / Register Page

* User authentication
* Account creation

#### Product Listing Page

* Displays all available products
* Supports filtering and browsing

#### Product Detail Page

* Detailed information about a selected product
* Pricing, description, and availability

#### Cart Page

* View selected items
* Update quantities or remove items

#### Checkout Page

* Enter shipping and payment details
* Confirm purchase

#### Sales / Discounts Display

* Highlights active promotions
* Applies discounts dynamically

#### API Integration

* Connects to backend services via REST APIs

---

## Backend Architecture

The backend follows a layered architecture with **Managers** and **Engines**:

---

### Managers (Business Logic Layer)

Managers handle high-level operations and coordinate workflows.

* **UserManager** – Handles user authentication and management
* **CartManager** – Manages cart operations
* **ProductManager** – Controls product-related logic
* **CheckoutManager** – Handles checkout flow
* **SaleManager** – Applies and manages discounts
* **OrderManager** – Processes and tracks orders

---

### Engines (Core Processing Layer)

Engines perform lower-level logic and data operations.

* **UserEngine** – Core user operations
* **CartEngine** – Cart data handling
* **ProductEngine** – Product data processing
* **CheckoutEngine** – Checkout execution logic
* **SaleEngine** – Discount calculations
* **OrderEngine** – Order persistence and processing

---

## Data Flow

1. User interacts with React frontend
2. Frontend sends requests to backend API
3. Managers process requests and coordinate logic
4. Engines execute core operations
5. Response is returned to frontend

---

## Getting Started

### Prerequisites

* Node.js
* .NET SDK
* Git

---

### Frontend Setup

```bash
git clone <repo-url>
cd frontend
npm install
npm start
```

---

### Backend Setup

```bash
cd backend
dotnet restore
dotnet run
```

---

## API Integration

Ensure the frontend is configured to communicate with the backend API:

* Base URL example:

```
http://localhost:5000/api
```

---

## Future Improvements

* Add authentication with JWT
* Implement payment gateway integration
* Improve UI/UX design
* Add unit and integration tests
* Optimize performance and caching

---

## License

This project is for educational purposes.

---

## Contributing

Contributions are welcome! Feel free to fork the repo and submit a pull request.

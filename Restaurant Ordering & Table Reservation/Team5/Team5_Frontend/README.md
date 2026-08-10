# Team5 Spoons Frontend

Bootstrap HTML/CSS/JavaScript frontend for the Restaurant Ordering & Table Reservation capstone.

## Setup
1. Start the Team5 ASP.NET Core Web API.
2. Open `js/api.js`.
3. Change `API_BASE_URL` to the exact HTTPS URL printed by `dotnet run` (for example `https://localhost:7xxx`).
4. If the frontend is served from another origin, enable CORS in the ASP.NET Core backend.
5. Open `index.html` using Live Server or another local static server.

## Included
- Login and registration
- JWT saved in localStorage
- Automatic `Authorization: Bearer <token>` header
- Bootstrap responsive UI
- Dashboard
- CRUD UI for User, MenuCategory, MenuItem, Table, Reservation, Order, OrderItem, Payment, Ingredient, MenuItemIngredient and Review
- Find/view, filtering, sorting and count UI
- Foreign-key fields for related entities
- Logout

## Backend route assumption
Controllers use the standard routes:
`/api/Auth/login`, `/api/Auth/register`, `/api/{ControllerName}`.

If your actual controller route differs, edit `endpoint` in `js/app.js`.

## Important
The capstone requirement says the frontend must fully exercise every controller, use Bootstrap, include JWT login/registration and attach JWT to protected requests. The backend remains responsible for reservation-confirmation and paid-order receipt email triggers.

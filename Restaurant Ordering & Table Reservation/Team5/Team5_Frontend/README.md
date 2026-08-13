# spoons — Frontend (Team 5)

A simple Bootstrap website for the Restaurant Ordering & Table Reservation API.
**Mostly HTML.** Each screen is one `.html` file you can read and edit. There is
only **one** shared JavaScript file (`js/app.js`), plus a short `<script>` at the
bottom of each page.

## How to run

1. **Start the backend** (from the `Team5` project folder):
   ```bash
   dotnet run --launch-profile http
   ```
   It runs on `http://localhost:5073`.

2. **Serve this folder** (so the pages can talk to the backend):
   ```bash
   python -m http.server 5500
   ```
   Then open **http://localhost:5500/login.html** in your browser.
   (Don't open the files by double-clicking — always use the address above.)

3. If your backend uses a different port, change `API_BASE` at the top of
   [`js/app.js`](js/app.js).

**Sign in:** admin `admin@spoons.com / admin123`, customer `sara@spoons.com / sara123`,
or click “Create account”.

## The pages (each is one .html file you can read)

**Customer**
| File | Page |
|------|------|
| `login.html` | Sign in / register |
| `index.html` | Home |
| `menu.html` | Browse menu (search + category filter) |
| `reserve.html` | Book a table |
| `cart.html` | Cart + checkout (makes the order + payment) |
| `orders.html` | My orders |
| `reservations.html` | My reservations |
| `profile.html` | My account |

**Admin**
`admin-dashboard.html`, `admin-users.html`, `admin-categories.html`,
`admin-menu-items.html`, `admin-tables.html`, `admin-reservations.html`,
`admin-orders.html`, `admin-payments.html`, `admin-reviews.html`,
`admin-ingredients.html`

## How each page is built (this is the whole idea)

Every page has three parts, all in the same file:

1. **The navbar** — plain HTML at the top.
2. **The content** — plain HTML (headings, tables, forms). Places that need data
   from the server are left empty, e.g. `<tbody id="rows"></tbody>`.
3. **A little script** at the bottom that fills those empty places by calling the
   server. It always looks the same:
   ```html
   <script src="js/app.js"></script>
   <script>
     requireLogin();                       // or requireAdmin()
     async function load() {
       const data = await api("GET", EP.items);   // ask the server
       document.getElementById("rows").innerHTML = data.map(...).join("");
     }
     load();
   </script>
   ```

## The one shared file: `js/app.js`
It gives every page these helpers so the page scripts stay short:
- `api("GET"/"POST"/"PUT"/"DELETE", address, body)` — talk to the backend (adds your login token)
- `EP` — the list of backend addresses
- `saveLogin`, `me`, `logout`, `requireLogin`, `requireAdmin` — who is logged in
- `Cart` — the shopping cart
- `esc`, `money`, `badge` — small formatting helpers

## Backend fixes that were needed
Small changes were made to the API so the frontend can use it: registered
`JwtService`, made model IDs appear in responses (they were hidden), stopped
navigation properties from being “required”, and fixed the user/category update
methods. See the project notes for details.

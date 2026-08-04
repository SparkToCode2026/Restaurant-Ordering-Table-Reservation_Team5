const {
  Document, Packer, Paragraph, TextRun, HeadingLevel, Table, TableRow, TableCell,
  WidthType, ShadingType, BorderStyle, AlignmentType,
} = require("docx");
const fs = require("fs");

const BRAND_RED = "B3202B";
const BRAND_BLACK = "1A1A1A";
const GRAY = "6B6B6B";

function h1(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_1,
    spacing: { before: 240, after: 100 },
    border: { bottom: { style: BorderStyle.SINGLE, size: 12, color: BRAND_RED, space: 6 } },
    children: [new TextRun({ text, color: BRAND_BLACK })],
  });
}
function h2(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_2,
    spacing: { before: 200, after: 80 },
    children: [new TextRun({ text, color: BRAND_RED })],
  });
}
function p(text, opts = {}) {
  return new Paragraph({ spacing: { after: 100 }, children: [new TextRun({ text, size: 21, ...opts })] });
}
function bullet(text) {
  return new Paragraph({ spacing: { after: 40 }, children: [new TextRun({ text: "•  " + text, size: 21 })] });
}
function code(text) {
  return new Paragraph({
    spacing: { after: 100 },
    children: [new TextRun({ text, font: "Consolas", size: 19, color: "2D6A4F" })],
  });
}

function cell(text, opts = {}) {
  return new TableCell({
    width: { size: opts.width || 2000, type: WidthType.DXA },
    shading: opts.header ? { type: ShadingType.CLEAR, fill: "F3D9DC" } : undefined,
    children: [new Paragraph({ children: [new TextRun({ text, bold: !!opts.header, size: 19 })] })],
  });
}

function fieldTable(rows) {
  const widths = [2000, 1600, 5900];
  return new Table({
    width: { size: 9500, type: WidthType.DXA },
    columnWidths: widths,
    rows: [
      new TableRow({ children: [cell("Field", { header: true, width: widths[0] }), cell("Type", { header: true, width: widths[1] }), cell("Notes", { header: true, width: widths[2] })] }),
      ...rows.map((r) => new TableRow({ children: [cell(r[0], { width: widths[0] }), cell(r[1], { width: widths[1] }), cell(r[2], { width: widths[2] })] })),
    ],
  });
}

const doc = new Document({
  sections: [
    {
      properties: { page: { size: { width: 12240, height: 15840 }, margin: { top: 1000, bottom: 1000, left: 1100, right: 1100 } } },
      children: [
        new Paragraph({ spacing: { after: 40 }, children: [new TextRun({ text: "S P O O N S", bold: true, size: 40, color: BRAND_RED })] }),
        new Paragraph({ spacing: { after: 200 }, children: [new TextRun({ text: "by FiveMinds", italics: true, size: 24, color: BRAND_BLACK })] }),
        new Paragraph({ spacing: { after: 40 }, children: [new TextRun({ text: "EF Core Code — Individual Task Submission", bold: true, size: 30 })] }),
        new Paragraph({ spacing: { after: 300 }, children: [new TextRun({ text: "Restaurant Ordering & Table Reservation · Codeline Spark to Code 2026", size: 21, color: GRAY })] }),

        h1("1. Assignment"),
        p("Assigned models: Reservation and OrderItem."),
        p("Scope of this task: write the EF Core entity classes for these two models and their Fluent API configuration (mapping, relationships, delete behavior, indexes). These files are submitted on their own — they plug into the team's shared ApplicationDbContext once merged; they do not modify anything outside the two assigned models."),

        h1("2. Reservation"),
        p("A customer's booking of a table for a given date, time, and party size."),
        fieldTable([
          ["Id", "int (PK)", "Identity column."],
          ["UserId", "int (FK)", "Required. References User — the customer who booked."],
          ["TableId", "int (FK)", "Required. References Table — the table being reserved."],
          ["ReservationDate", "DateOnly", "Calendar date of the booking."],
          ["ReservationTime", "TimeOnly", "Time of the booking."],
          ["PartySize", "int", "Number of guests."],
          ["Status", "ReservationStatus (enum)", "Pending → Confirmed → Completed, or Cancelled. Defined in the shared Enums.cs."],
          ["CreatedAt", "DateTime", "UTC timestamp, set on insert."],
        ]),
        h2("Relationships"),
        bullet("Reservation N — 1 User (many-to-one, required): one customer can have many reservations."),
        bullet("Reservation N — 1 Table (many-to-one, required): one table can have many reservations over time."),
        h2("Design decisions"),
        bullet("Both FKs are required — a reservation cannot exist without a customer and a table."),
        bullet("DeleteBehavior.Restrict on both relationships: a User or Table with existing reservations cannot be deleted outright. This protects booking history and forces an explicit cancel/reassign step first, instead of silently cascading deletes into a customer's reservation record."),
        bullet("Composite index on (TableId, ReservationDate, ReservationTime) to make the 'is this table free at this date/time' availability check and the per-table upcoming-reservations lookup efficient."),

        h1("3. OrderItem"),
        p("A single line item within an Order — one menu item plus a quantity."),
        fieldTable([
          ["Id", "int (PK)", "Identity column."],
          ["OrderId", "int (FK)", "Required. References Order — the parent order."],
          ["MenuItemId", "int (FK)", "Required. References MenuItem — the item ordered."],
          ["Quantity", "int", "Units ordered."],
          ["UnitPrice", "decimal(10,2)", "Snapshot of MenuItem.Price at order time."],
          ["Subtotal", "decimal(10,2)", "UnitPrice × Quantity, stored (not computed on read)."],
        ]),
        h2("Relationships"),
        bullet("OrderItem N — 1 Order (many-to-one, required): one order can have many line items."),
        bullet("OrderItem N — 1 MenuItem (many-to-one, required): one menu item can appear on many order lines."),
        h2("Design decisions"),
        bullet("UnitPrice is captured at order time rather than read live from MenuItem.Price, so a later menu price change never retroactively changes historical order totals."),
        bullet("DeleteBehavior.Cascade from Order → OrderItem: a line item has no meaning outside its parent order, so deleting an order deletes its lines with it."),
        bullet("DeleteBehavior.Restrict from MenuItem → OrderItem: a menu item that has ever been ordered cannot be hard-deleted, since that would corrupt order history. It should be deactivated (IsAvailable = false) instead of removed."),
        bullet("Separate indexes on OrderId and MenuItemId support the order-detail view (all lines for an order) and reporting queries (how many times an item has been ordered)."),

        h1("4. Files in this submission"),
        code("Models/Reservation.cs"),
        code("Models/OrderItem.cs"),
        code("Data/Configurations/ReservationConfiguration.cs"),
        code("Data/Configurations/OrderItemConfiguration.cs"),
        p("Both configuration classes implement IEntityTypeConfiguration<T> rather than being added inline to the shared ApplicationDbContext.OnModelCreating method. This was a deliberate choice for team workflow: with several people assigned different models in the same DbContext file, inline Fluent API blocks are a common source of merge conflicts. A separate config class per model can be added, reviewed, and merged independently."),

        h1("5. Integration instructions (for whoever merges this in)"),
        p("1. Copy Models/Reservation.cs and Models/OrderItem.cs into RestaurantApi/Models/ (they assume User, Table, Order, MenuItem, and the ReservationStatus enum already exist there, as per the shared ERD)."),
        p("2. Copy Data/Configurations/ReservationConfiguration.cs and OrderItemConfiguration.cs into RestaurantApi/Data/Configurations/."),
        p("3. In ApplicationDbContext.OnModelCreating, either:"),
        code("modelBuilder.ApplyConfiguration(new ReservationConfiguration());"),
        code("modelBuilder.ApplyConfiguration(new OrderItemConfiguration());"),
        p("   — or, to pick up every configuration class in the project automatically:"),
        code("modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);"),
        p("4. Ensure DbSet<Reservation> and DbSet<OrderItem> are exposed on ApplicationDbContext (already present in the shared context)."),
        p("5. Run a migration (dotnet ef migrations add ...) once merged with the rest of the team's models."),

        new Paragraph({
          spacing: { before: 300 },
          border: { top: { style: BorderStyle.SINGLE, size: 6, color: "CCCCCC", space: 6 } },
          children: [new TextRun({ text: "Prepared for team review before Mentor approval and Slack distribution of the final Restaurant_ERD_and_Data_Mapping.  —  Spoons by FiveMinds", size: 18, color: GRAY, italics: true })],
        }),
      ],
    },
  ],
});

Packer.toBuffer(doc).then((buf) => {
  fs.writeFileSync("EFCore_Task_Reservation_OrderItem_Design_Notes.docx", buf);
  console.log("wrote docx");
});

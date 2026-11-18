import { useMemo, useState } from "react";
import LogoMark from "../../../shared/components/LogoMark";
import "./DashboardPage.css";

const initialBookings = [
  {
    id: "ST-1048",
    item: "DJI Mavic 3 Classic",
    customer: "Alex Morgan",
    start: "18 Nov, 09:00",
    end: "20 Nov, 18:00",
    status: "Confirmed",
    total: 48500,
  },
  {
    id: "ST-1047",
    item: "Canon EOS R5",
    customer: "Sarah Chen",
    start: "18 Nov, 11:30",
    end: "19 Nov, 16:00",
    status: "Pending",
    total: 32000,
  },
  {
    id: "ST-1046",
    item: "Sony A7 IV Kit",
    customer: "James Wilson",
    start: "17 Nov, 08:00",
    end: "21 Nov, 17:30",
    status: "Active",
    total: 67500,
  },
  {
    id: "ST-1045",
    item: "MacBook Pro 16”",
    customer: "Nina Patel",
    start: "16 Nov, 10:00",
    end: "18 Nov, 10:00",
    status: "Returned",
    total: 29400,
  },
  {
    id: "ST-1044",
    item: "GoPro HERO12 Bundle",
    customer: "Daniel Lee",
    start: "15 Nov, 13:00",
    end: "16 Nov, 20:00",
    status: "Cancelled",
    total: 12800,
  },
];

function Icon({ name, size = 20 }) {
  const paths = {
    grid: '<rect x="3" y="3" width="7" height="7" rx="2"/><rect x="14" y="3" width="7" height="7" rx="2"/><rect x="3" y="14" width="7" height="7" rx="2"/><rect x="14" y="14" width="7" height="7" rx="2"/>',
    calendar:
      '<rect x="3" y="5" width="18" height="16" rx="3"/><path d="M16 3v4M8 3v4M3 10h18"/>',
    box: '<path d="M21 8l-9-5-9 5 9 5 9-5Z"/><path d="M3 8v9l9 5 9-5V8M12 13v9"/>',
    users:
      '<path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75"/>',
    chart: '<path d="M3 3v18h18"/><path d="M7 16l4-5 3 3 6-7"/>',
    plus: '<path d="M12 5v14M5 12h14"/>',
    search: '<circle cx="11" cy="11" r="7"/><path d="M20 20l-4-4"/>',
    bell: '<path d="M18 8a6 6 0 0 0-12 0c0 7-3 7-3 9h18c0-2-3-2-3-9M13.73 21a2 2 0 0 1-3.46 0"/>',
    arrow: '<path d="M5 12h14M13 6l6 6-6 6"/>',
    truck:
      '<path d="M10 17h4V5H2v12h3M14 9h4l4 4v4h-3"/><circle cx="7" cy="17" r="2"/><circle cx="17" cy="17" r="2"/>',
    clock: '<circle cx="12" cy="12" r="9"/><path d="M12 7v5l3 2"/>',
    check: '<circle cx="12" cy="12" r="9"/><path d="M8 12l3 3 5-6"/>',
    close: '<path d="M6 6l12 12M18 6L6 18"/>',
  };

  return (
    <svg
      width={size}
      height={size}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="1.8"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
      dangerouslySetInnerHTML={{ __html: paths[name] }}
    />
  );
}

function DashboardPage() {
  const [bookings, setBookings] = useState(initialBookings);
  const [filter, setFilter] = useState("All");
  const [search, setSearch] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);

  const visibleBookings = useMemo(
    () =>
      bookings.filter((booking) => {
        const matchesFilter = filter === "All" || booking.status === filter;
        const haystack =
          `${booking.id} ${booking.item} ${booking.customer}`.toLowerCase();
        return matchesFilter && haystack.includes(search.toLowerCase());
      }),
    [bookings, filter, search],
  );

  const createBooking = (event) => {
    event.preventDefault();
    const data = new FormData(event.currentTarget);
    const idNumber = 1049 + bookings.length - initialBookings.length;
    setBookings((current) => [
      {
        id: `ST-${idNumber}`,
        item: data.get("item"),
        customer: data.get("customer"),
        start: data.get("start"),
        end: data.get("end"),
        status: "Pending",
        total: Number(data.get("total")) || 0,
      },
      ...current,
    ]);
    setIsModalOpen(false);
  };

  return (
    <div className="booking-app">
      <aside className="booking-sidebar">
        <div className="sidebar-brand">
          <LogoMark />
          <span>
            Swift<span>Track</span>
          </span>
        </div>

        <nav className="booking-nav" aria-label="Main navigation">
          <button>
            <Icon name="grid" />
            Overview
          </button>
          <button className="active">
            <Icon name="calendar" />
            Bookings<span className="nav-count">12</span>
          </button>
          <button>
            <Icon name="box" />
            Equipment
          </button>
          <button>
            <Icon name="users" />
            Customers
          </button>
          <button>
            <Icon name="chart" />
            Reports
          </button>
        </nav>

        <div className="sidebar-profile">
          <div className="avatar">AM</div>
          <div>
            <strong>Alex Morgan</strong>
            <span>Administrator</span>
          </div>
          <button aria-label="Profile options">•••</button>
        </div>
      </aside>

      <main className="booking-main">
        <header className="booking-header">
          <div>
            <p className="page-kicker">OPERATIONS</p>
            <h1>Bookings</h1>
          </div>
          <div className="header-actions">
            <button className="icon-button" aria-label="Notifications">
              <Icon name="bell" />
              <span className="notification-dot" />
            </button>
            <button
              className="new-booking-button"
              onClick={() => setIsModalOpen(true)}
            >
              <Icon name="plus" size={18} />
              New booking
            </button>
          </div>
        </header>

        <section className="stats-grid" aria-label="Booking statistics">
          <article className="stat-card">
            <div className="stat-icon green">
              <Icon name="calendar" />
            </div>
            <div>
              <span>Total bookings</span>
              <strong>128</strong>
              <small>
                <b>+12%</b> this month
              </small>
            </div>
          </article>
          <article className="stat-card">
            <div className="stat-icon blue">
              <Icon name="clock" />
            </div>
            <div>
              <span>In progress</span>
              <strong>24</strong>
              <small>6 starting today</small>
            </div>
          </article>
          <article className="stat-card">
            <div className="stat-icon amber">
              <Icon name="truck" />
            </div>
            <div>
              <span>Ready for pickup</span>
              <strong>8</strong>
              <small>Next 2 hours</small>
            </div>
          </article>
          <article className="stat-card">
            <div className="stat-icon violet">
              <Icon name="check" />
            </div>
            <div>
              <span>Completed</span>
              <strong>96</strong>
              <small>
                <b>+8%</b> this month
              </small>
            </div>
          </article>
        </section>

        <section className="bookings-card">
          <div className="table-toolbar">
            <div
              className="filter-tabs"
              role="tablist"
              aria-label="Booking status filters"
            >
              {["All", "Pending", "Confirmed", "Active", "Returned"].map(
                (item) => (
                  <button
                    key={item}
                    className={filter === item ? "active" : ""}
                    onClick={() => setFilter(item)}
                  >
                    {item}
                  </button>
                ),
              )}
            </div>
            <label className="search-field">
              <Icon name="search" size={17} />
              <input
                value={search}
                onChange={(event) => setSearch(event.target.value)}
                placeholder="Search bookings"
              />
            </label>
          </div>

          <div className="table-wrap">
            <table>
              <thead>
                <tr>
                  <th>Booking</th>
                  <th>Equipment</th>
                  <th>Customer</th>
                  <th>Rental period</th>
                  <th>Status</th>
                  <th>Total</th>
                  <th />
                </tr>
              </thead>
              <tbody>
                {visibleBookings.map((booking) => (
                  <tr key={booking.id}>
                    <td>
                      <strong className="booking-id">#{booking.id}</strong>
                    </td>
                    <td>
                      <div className="equipment-cell">
                        <span className="equipment-thumb">
                          <Icon name="box" size={18} />
                        </span>
                        <strong>{booking.item}</strong>
                      </div>
                    </td>
                    <td>{booking.customer}</td>
                    <td>
                      <div className="period">
                        <span>{booking.start}</span>
                        <small>to {booking.end}</small>
                      </div>
                    </td>
                    <td>
                      <span
                        className={`status status-${booking.status.toLowerCase()}`}
                      >
                        <i />
                        {booking.status}
                      </span>
                    </td>
                    <td>
                      <strong className="total">
                        LKR {booking.total.toLocaleString()}
                      </strong>
                    </td>
                    <td>
                      <button
                        className="row-action"
                        aria-label={`View ${booking.id}`}
                      >
                        <Icon name="arrow" size={16} />
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            {visibleBookings.length === 0 && (
              <div className="empty-state">No bookings match your search.</div>
            )}
          </div>

          <footer className="table-footer">
            <span>
              Showing {visibleBookings.length} of {bookings.length} bookings
            </span>
            <div>
              <button disabled>‹</button>
              <button className="active">1</button>
              <button>2</button>
              <button>3</button>
              <button>›</button>
            </div>
          </footer>
        </section>
      </main>

      {isModalOpen && (
        <div
          className="modal-backdrop"
          role="presentation"
          onMouseDown={(event) =>
            event.target === event.currentTarget && setIsModalOpen(false)
          }
        >
          <section
            className="booking-modal"
            role="dialog"
            aria-modal="true"
            aria-labelledby="modal-title"
          >
            <div className="modal-heading">
              <div>
                <p className="page-kicker">NEW REQUEST</p>
                <h2 id="modal-title">Create booking</h2>
              </div>
              <button onClick={() => setIsModalOpen(false)} aria-label="Close">
                <Icon name="close" />
              </button>
            </div>
            <form onSubmit={createBooking}>
              <label>
                Customer name
                <input
                  name="customer"
                  placeholder="Enter customer name"
                  required
                />
              </label>
              <label>
                Equipment
                <input
                  name="item"
                  placeholder="Select or enter equipment"
                  required
                />
              </label>
              <div className="modal-row">
                <label>
                  Start date and time
                  <input name="start" type="datetime-local" required />
                </label>
                <label>
                  End date and time
                  <input name="end" type="datetime-local" required />
                </label>
              </div>
              <label>
                Estimated total (LKR)
                <input
                  name="total"
                  type="number"
                  min="0"
                  placeholder="0"
                  required
                />
              </label>
              <div className="modal-actions">
                <button type="button" onClick={() => setIsModalOpen(false)}>
                  Cancel
                </button>
                <button type="submit">Create booking</button>
              </div>
            </form>
          </section>
        </div>
      )}
    </div>
  );
}

export default DashboardPage;

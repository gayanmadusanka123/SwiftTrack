import { useMemo, useState } from 'react'
import './AdminDashboardPage.css'

const initialEquipment = [
  { id: 1, name: 'Sony FX6 Cinema Camera', category: 'Camera', sku: 'CAM-SFX6-001', location: 'A-12-04', status: 'Available', quantity: 8, value: '$19,200' },
  { id: 2, name: 'Canon RF 24-70mm F2.8L', category: 'Lens', sku: 'LEN-RF2470-02', location: 'B-04-12', status: 'Rented', quantity: 14, value: '$28,700' },
  { id: 3, name: 'ARRI SkyPanel S60-C', category: 'Lighting', sku: 'LGT-ASP60C-11', location: 'C-02-08', status: 'Available', quantity: 6, value: '$14,400' },
  { id: 4, name: 'DJI RS 3 Pro Gimbal', category: 'Support', sku: 'SUP-RS3P-07', location: 'A-08-03', status: 'Maintenance', quantity: 3, value: '$2,697' },
  { id: 5, name: 'Sennheiser MKH 416', category: 'Audio', sku: 'AUD-MKH416-05', location: 'D-01-16', status: 'Available', quantity: 10, value: '$10,900' },
]

const initialDrivers = [
  { id: 1, name: 'Ethan Carter', initials: 'EC', route: 'Downtown → Studio 14', status: 'On route', time: 'Arrives in 18 min', color: 'blue' },
  { id: 2, name: 'Maya Rodriguez', initials: 'MR', route: 'Warehouse → Film City', status: 'Available', time: 'Ready for dispatch', color: 'purple' },
  { id: 3, name: 'Noah Williams', initials: 'NW', route: 'Northside → Warehouse', status: 'On break', time: 'Back at 02:15 PM', color: 'orange' },
]

const navItems = [
  ['overview', 'Overview', 'grid'],
  ['inventory', 'Inventory', 'box'],
  ['orders', 'Orders', 'clipboard'],
  ['drivers', 'Drivers', 'truck'],
  ['customers', 'Customers', 'users'],
]

function Icon({ name, size = 18 }) {
  const paths = {
    grid: <><rect x="3" y="3" width="7" height="7" rx="1" /><rect x="14" y="3" width="7" height="7" rx="1" /><rect x="3" y="14" width="7" height="7" rx="1" /><rect x="14" y="14" width="7" height="7" rx="1" /></>,
    box: <><path d="m21 8-9 5-9-5 9-5 9 5Z" /><path d="M3 8v9l9 5 9-5V8M12 13v9" /></>,
    clipboard: <><rect x="5" y="4" width="14" height="17" rx="2" /><path d="M9 4.5V3h6v1.5M9 10h6M9 14h4" /></>,
    truck: <><path d="M3 6h11v10H3zM14 10h4l3 3v3h-7z" /><circle cx="7" cy="18" r="2" /><circle cx="18" cy="18" r="2" /></>,
    users: <><circle cx="9" cy="8" r="3" /><path d="M3 20c0-3.3 2.7-5 6-5s6 1.7 6 5M16 5.2a3 3 0 0 1 0 5.6M18 15c2.2.4 3 2 3 4" /></>,
    settings: <><circle cx="12" cy="12" r="3" /><path d="M19.4 15a1.7 1.7 0 0 0 .3 1.9l.1.1-1.7 1.7-.1-.1a1.7 1.7 0 0 0-1.9-.3 1.7 1.7 0 0 0-1 1.6v.1h-2.4v-.1a1.7 1.7 0 0 0-1-1.6 1.7 1.7 0 0 0-1.9.3l-.1.1L8 17l.1-.1A1.7 1.7 0 0 0 8.4 15a1.7 1.7 0 0 0-1.6-1H6v-2.4h.8a1.7 1.7 0 0 0 1.6-1A1.7 1.7 0 0 0 8 8.7L8 8.6l1.7-1.7.1.1a1.7 1.7 0 0 0 1.9.3 1.7 1.7 0 0 0 1-1.6v-.1h2.4v.1a1.7 1.7 0 0 0 1 1.6 1.7 1.7 0 0 0 1.9-.3l.1-.1 1.7 1.7-.1.1a1.7 1.7 0 0 0-.3 1.9 1.7 1.7 0 0 0 1.6 1h.1V14h-.1a1.7 1.7 0 0 0-1.6 1Z" /></>,
    search: <><circle cx="11" cy="11" r="7" /><path d="m20 20-4-4" /></>,
    bell: <><path d="M18 9a6 6 0 0 0-12 0c0 7-3 7-3 9h18c0-2-3-2-3-9ZM10 22h4" /></>,
    plus: <><path d="M12 5v14M5 12h14" /></>,
    arrow: <><path d="M5 12h14M13 6l6 6-6 6" /></>,
    more: <><circle cx="5" cy="12" r="1" fill="currentColor" stroke="none" /><circle cx="12" cy="12" r="1" fill="currentColor" stroke="none" /><circle cx="19" cy="12" r="1" fill="currentColor" stroke="none" /></>,
    close: <><path d="m6 6 12 12M18 6 6 18" /></>,
  }
  return <svg className="icon" width={size} height={size} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">{paths[name]}</svg>
}

function AdminDashboardPage() {
  const [activePage, setActivePage] = useState('overview')
  const [equipment, setEquipment] = useState(initialEquipment)
  const [drivers, setDrivers] = useState(initialDrivers)
  const [query, setQuery] = useState('')
  const [modal, setModal] = useState(null)
  const [form, setForm] = useState({})

  const filteredEquipment = useMemo(() => equipment.filter((item) =>
    `${item.name} ${item.sku} ${item.category}`.toLowerCase().includes(query.toLowerCase())
  ), [equipment, query])

  const openEquipmentModal = (item) => {
    setForm(item || { name: '', category: 'Camera', sku: '', location: '', status: 'Available', quantity: 1, value: '$0' })
    setModal('equipment')
  }

  const saveEquipment = (event) => {
    event.preventDefault()
    if (!form.name.trim() || !form.sku.trim()) return
    setEquipment((current) => form.id ? current.map((item) => item.id === form.id ? { ...form, quantity: Number(form.quantity) } : item) : [{ ...form, id: Date.now(), quantity: Number(form.quantity) }, ...current])
    setModal(null)
  }

  const saveDriver = (event) => {
    event.preventDefault()
    if (!form.name.trim()) return
    const initials = form.name.split(' ').map((part) => part[0]).join('').slice(0, 2).toUpperCase()
    setDrivers((current) => [...current, { id: Date.now(), name: form.name, initials, route: 'Awaiting assignment', status: 'Available', time: 'Ready for dispatch', color: 'green' }])
    setModal(null)
  }

  return (
    <div className="admin-shell">
      <aside className="admin-sidebar">
        <div className="admin-brand"><span className="admin-brand-mark">S</span><span>swift<span>track</span></span></div>
        <div className="workspace-switcher"><div className="workspace-icon">W</div><div><strong>Westside Warehouse</strong><small>Admin workspace</small></div><span className="chevron">⌄</span></div>
        <p className="nav-label">Workspace</p>
        <nav className="admin-nav-list">{navItems.map(([id, label, icon]) => <button key={id} className={`admin-nav ${activePage === id ? 'active' : ''}`} onClick={() => setActivePage(id)}><Icon name={icon} /><span>{label}</span>{id === 'orders' && <b className="nav-count">12</b>}</button>)}</nav>
        <div className="sidebar-bottom"><button className={`admin-nav ${activePage === 'settings' ? 'active' : ''}`} onClick={() => setActivePage('settings')}><Icon name="settings" /><span>Settings</span></button><div className="user-card"><div className="avatar avatar-green">JD</div><div><strong>James Davis</strong><small>Administrator</small></div><Icon name="more" size={16} /></div></div>
      </aside>

      <main className="admin-main">
        <header className="topbar"><div className="mobile-brand"><span className="admin-brand-mark">S</span><strong>swift<span>track</span></strong></div><div className="topbar-actions"><label className="global-search"><Icon name="search" size={17} /><input placeholder="Search anything..." value={query} onChange={(event) => setQuery(event.target.value)} /><kbd>⌘ K</kbd></label><button className="icon-button notification"><Icon name="bell" size={19} /><i /></button><div className="avatar avatar-green">JD</div></div></header>
        <div className="content">
          <div className="page-heading"><div><p className="eyebrow">Monday, October 14, 2024</p><h1>{activePage === 'overview' ? <>Good morning, James <span>✦</span></> : navItems.find(([id]) => id === activePage)?.[1] || 'Settings'}</h1><p className="heading-subtitle">{activePage === 'overview' ? 'Here’s what’s happening across your warehouse today.' : `Manage your ${activePage === 'settings' ? 'workspace preferences' : activePage} from one place.`}</p></div>{(activePage === 'overview' || activePage === 'inventory') && <button className="primary-button" onClick={() => openEquipmentModal()}><Icon name="plus" size={17} /> Add equipment</button>}{activePage === 'drivers' && <button className="primary-button" onClick={() => { setForm({ name: '' }); setModal('driver') }}><Icon name="plus" size={17} /> Add driver</button>}</div>

          {activePage !== 'overview' && <section className="panel page-view-panel">
            {activePage === 'inventory' && <><div className="panel-heading"><div><h2>All inventory</h2><p>Search, update, and manage every rental item in the warehouse.</p></div><span className="inventory-total">{equipment.length} items</span></div><div className="table-wrap"><table><thead><tr><th>Equipment</th><th>SKU</th><th>Location</th><th>Status</th><th>Qty.</th><th>Value</th><th /></tr></thead><tbody>{filteredEquipment.map((item) => <tr key={item.id}><td><div className="equipment-name"><span className={`equipment-icon ${item.category.toLowerCase()}`}>{item.category === 'Camera' ? '◉' : item.category === 'Lens' ? '◌' : item.category === 'Lighting' ? '✦' : item.category === 'Audio' ? '◖' : '＋'}</span><div><strong>{item.name}</strong><small>{item.category}</small></div></div></td><td className="muted">{item.sku}</td><td className="muted">{item.location}</td><td><span className={`status-pill ${item.status.toLowerCase()}`}>{item.status}</span></td><td>{item.quantity}</td><td><strong>{item.value}</strong></td><td><button className="row-menu" onClick={() => openEquipmentModal(item)}><Icon name="more" size={17} /></button></td></tr>)}</tbody></table></div></>}
            {activePage === 'drivers' && <><div className="panel-heading"><div><h2>Delivery team</h2><p>Monitor driver availability and active routes.</p></div><span className="inventory-total">{drivers.length} active records</span></div><div className="drivers-list">{drivers.map((driver) => <div className="driver-row" key={driver.id}><div className={`avatar avatar-${driver.color}`}>{driver.initials}</div><div className="driver-info"><strong>{driver.name}</strong><span>{driver.route}</span></div><span className={`driver-status ${driver.status.toLowerCase().replace(' ', '-')}`}><i />{driver.status}</span><span className="driver-time">{driver.time}</span></div>)}</div></>}
            {(activePage === 'orders' || activePage === 'customers' || activePage === 'settings') && <div className="empty-page"><div className="empty-icon"><Icon name={activePage === 'orders' ? 'clipboard' : activePage === 'customers' ? 'users' : 'settings'} size={25} /></div><h2>{activePage === 'orders' ? 'Orders are ready to manage' : activePage === 'customers' ? 'Customer management' : 'Workspace settings'}</h2><p>This section is connected to the sidebar and ready for your warehouse workflow.</p><button className="primary-button" onClick={() => setActivePage('overview')}>Back to overview</button></div>}
          </section>}

          {activePage === 'overview' && <>
          <section className="stats-grid">
            <article className="stat-card"><div className="stat-icon mint"><Icon name="box" /></div><div><p>Total equipment</p><strong>248</strong><span className="trend up">↗ 12.5%</span><small>vs. last month</small></div><div className="sparkline green"><i /><i /><i /><i /><i /><i /><i /></div></article>
            <article className="stat-card"><div className="stat-icon peach"><Icon name="clipboard" /></div><div><p>Active rentals</p><strong>86</strong><span className="trend up">↗ 8.2%</span><small>vs. last month</small></div><div className="sparkline orange"><i /><i /><i /><i /><i /><i /><i /></div></article>
            <article className="stat-card"><div className="stat-icon lavender"><Icon name="truck" /></div><div><p>Out for delivery</p><strong>18</strong><span className="trend down">↘ 3.1%</span><small>vs. last week</small></div><div className="sparkline purple"><i /><i /><i /><i /><i /><i /><i /></div></article>
            <article className="stat-card"><div className="stat-icon yellow"><Icon name="users" /></div><div><p>Available drivers</p><strong>{drivers.filter((driver) => driver.status === 'Available').length + 5}</strong><span className="trend neutral">● On schedule</span><small>of 12 drivers</small></div><div className="sparkline blue"><i /><i /><i /><i /><i /><i /><i /></div></article>
          </section>

          <section className="dashboard-grid">
            <article className="panel utilization-panel"><div className="panel-heading"><div><h2>Warehouse utilization</h2><p>Equipment occupancy across all zones</p></div><button className="select-button">Last 7 days <span>⌄</span></button></div><div className="chart-area"><div className="y-axis"><span>100%</span><span>75%</span><span>50%</span><span>25%</span><span>0%</span></div><div className="chart"><div className="grid-lines"><i /><i /><i /><i /><i /></div><svg viewBox="0 0 600 170" preserveAspectRatio="none"><defs><linearGradient id="chartFill" x1="0" x2="0" y1="0" y2="1"><stop offset="0" stopColor="#31b97b" stopOpacity=".25" /><stop offset="1" stopColor="#31b97b" stopOpacity="0" /></linearGradient></defs><path d="M0 130 C35 128 43 112 78 115 S126 102 160 110 S205 78 240 88 S283 95 315 70 S358 83 390 67 S430 47 460 60 S495 40 530 46 S570 24 600 30 V170 H0Z" fill="url(#chartFill)" /><path d="M0 130 C35 128 43 112 78 115 S126 102 160 110 S205 78 240 88 S283 95 315 70 S358 83 390 67 S430 47 460 60 S495 40 530 46 S570 24 600 30" fill="none" stroke="#1b9b66" strokeWidth="3" /></svg><div className="x-axis"><span>Oct 08</span><span>Oct 09</span><span>Oct 10</span><span>Oct 11</span><span>Oct 12</span><span>Oct 13</span><span>Oct 14</span></div></div></div><div className="chart-legend"><span><i className="legend-dot green-dot" /> Utilization</span><strong>78.4% <em>↗ 4.8%</em></strong></div></article>
            <article className="panel status-panel"><div className="panel-heading"><div><h2>Inventory status</h2><p>Current equipment breakdown</p></div><button className="more-button"><Icon name="more" /></button></div><div className="donut-wrap"><div className="donut"><div><strong>248</strong><span>Total items</span></div></div><div className="status-list"><div><i className="dot available" /><span>Available</span><strong>142 <small>57.3%</small></strong></div><div><i className="dot rented" /><span>Rented out</span><strong>86 <small>34.7%</small></strong></div><div><i className="dot maintenance" /><span>Maintenance</span><strong>20 <small>8.0%</small></strong></div></div></div><button className="text-button" onClick={() => setActivePage('inventory')}>View all inventory <Icon name="arrow" size={15} /></button></article>
          </section>

          <section className="panel inventory-panel"><div className="panel-heading"><div><h2>Recent inventory</h2><p>Manage your warehouse equipment and stock levels</p></div><div className="heading-actions"><button className="outline-button" onClick={() => setActivePage('inventory')}>View all</button><button className="primary-button small" onClick={() => openEquipmentModal()}><Icon name="plus" size={15} /> Add item</button></div></div><div className="table-wrap"><table><thead><tr><th>Equipment</th><th>SKU</th><th>Location</th><th>Status</th><th>Qty.</th><th>Value</th><th /></tr></thead><tbody>{filteredEquipment.slice(0, 5).map((item) => <tr key={item.id}><td><div className="equipment-name"><span className={`equipment-icon ${item.category.toLowerCase()}`}>{item.category === 'Camera' ? '◉' : item.category === 'Lens' ? '◌' : item.category === 'Lighting' ? '✦' : item.category === 'Audio' ? '◖' : '＋'}</span><div><strong>{item.name}</strong><small>{item.category}</small></div></div></td><td className="muted">{item.sku}</td><td className="muted">{item.location}</td><td><span className={`status-pill ${item.status.toLowerCase()}`}>{item.status}</span></td><td>{item.quantity}</td><td><strong>{item.value}</strong></td><td><button className="row-menu" onClick={() => openEquipmentModal(item)}><Icon name="more" size={17} /></button></td></tr>)}</tbody></table></div></section>

          <section className="panel drivers-panel"><div className="panel-heading"><div><h2>Driver overview</h2><p>Real-time status of your delivery team</p></div><div className="heading-actions"><button className="outline-button" onClick={() => setActivePage('drivers')}>Manage drivers</button><button className="primary-button small" onClick={() => { setForm({ name: '' }); setModal('driver') }}><Icon name="plus" size={15} /> Add driver</button></div></div><div className="drivers-list">{drivers.slice(0, 3).map((driver) => <div className="driver-row" key={driver.id}><div className={`avatar avatar-${driver.color}`}>{driver.initials}</div><div className="driver-info"><strong>{driver.name}</strong><span>{driver.route}</span></div><span className={`driver-status ${driver.status.toLowerCase().replace(' ', '-')}`}><i />{driver.status}</span><span className="driver-time">{driver.time}</span><button className="row-menu"><Icon name="more" size={17} /></button></div>)}</div></section>
          </>}
        </div>
      </main>

      {modal && <div className="modal-backdrop" onMouseDown={(event) => event.target === event.currentTarget && setModal(null)}><div className="modal"><div className="modal-header"><div><p className="eyebrow">{modal === 'driver' ? 'Driver management' : form.id ? 'Edit inventory' : 'New inventory item'}</p><h2>{modal === 'driver' ? 'Add a driver' : form.id ? 'Edit equipment' : 'Add equipment'}</h2></div><button className="close-button" onClick={() => setModal(null)}><Icon name="close" /></button></div>{modal === 'driver' ? <form onSubmit={saveDriver}><label>Driver name<input autoFocus required value={form.name || ''} onChange={(event) => setForm({ ...form, name: event.target.value })} placeholder="e.g. Alex Morgan" /></label><div className="modal-actions"><button type="button" className="outline-button" onClick={() => setModal(null)}>Cancel</button><button className="primary-button" type="submit">Add driver</button></div></form> : <form onSubmit={saveEquipment}><label>Equipment name<input autoFocus required value={form.name || ''} onChange={(event) => setForm({ ...form, name: event.target.value })} placeholder="e.g. Sony FX3 Cinema Camera" /></label><div className="form-two-col"><label>Category<select value={form.category} onChange={(event) => setForm({ ...form, category: event.target.value })}><option>Camera</option><option>Lens</option><option>Lighting</option><option>Audio</option><option>Support</option></select></label><label>SKU<input required value={form.sku || ''} onChange={(event) => setForm({ ...form, sku: event.target.value })} placeholder="CAM-0000" /></label></div><div className="form-two-col"><label>Location<input value={form.location || ''} onChange={(event) => setForm({ ...form, location: event.target.value })} placeholder="A-01-01" /></label><label>Quantity<input type="number" min="0" value={form.quantity || 0} onChange={(event) => setForm({ ...form, quantity: event.target.value })} /></label></div><div className="form-two-col"><label>Status<select value={form.status} onChange={(event) => setForm({ ...form, status: event.target.value })}><option>Available</option><option>Rented</option><option>Maintenance</option></select></label><label>Value<input value={form.value || ''} onChange={(event) => setForm({ ...form, value: event.target.value })} placeholder="$0" /></label></div><div className="modal-actions">{form.id && <button type="button" className="delete-button" onClick={() => { setEquipment((current) => current.filter((item) => item.id !== form.id)); setModal(null) }}>Delete item</button>}<span /><button type="button" className="outline-button" onClick={() => setModal(null)}>Cancel</button><button className="primary-button" type="submit">Save item</button></div></form>}</div></div>}
    </div>
  )
}

export default AdminDashboardPage

<template>
  <div class="app-shell">
    <aside class="sidebar">
      <div class="brand">
        <div class="brand-mark">P</div>
        <div>
          <h1>Personas App</h1>
          <p>Administración</p>
        </div>
      </div>

      <nav class="nav-links">
        <RouterLink to="/personas" class="nav-link">Listado</RouterLink>
        <RouterLink to="/personas/nueva" class="nav-link">Nueva persona</RouterLink>
      </nav>
    </aside>

    <div class="content-shell">
      <header class="topbar">
        <div>
          <p class="topbar-kicker">Sistema de gestión</p>
          <h2 class="topbar-title">Personas y direcciones</h2>
        </div>
      </header>

      <main class="page-container">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<style scoped>
:global(*) {
  box-sizing: border-box;
}

:global(body) {
  margin: 0;
  font-family: Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  color: #0f172a;
  background:
    radial-gradient(circle at top left, rgba(37, 99, 235, 0.08), transparent 25%),
    linear-gradient(180deg, #f8fbff 0%, #eef4ff 100%);
}

:global(a) {
  text-decoration: none;
}

.app-shell {
  min-height: 100vh;
  display: grid;
  grid-template-columns: 260px minmax(0, 1fr);
}

.sidebar {
  padding: 24px 18px;
  border-right: 1px solid rgba(226, 232, 240, 0.9);
  background: rgba(255, 255, 255, 0.75);
  backdrop-filter: blur(10px);
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 28px;
}

.brand-mark {
  width: 42px;
  height: 42px;
  border-radius: 14px;
  display: grid;
  place-items: center;
  font-weight: 900;
  color: white;
  background: linear-gradient(135deg, #2563eb, #0ea5e9);
  box-shadow: 0 12px 20px rgba(37, 99, 235, 0.2);
}

.brand h1 {
  margin: 0;
  font-size: 1rem;
}

.brand p {
  margin: 4px 0 0;
  color: #64748b;
  font-size: 0.85rem;
}

.nav-links {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.nav-link {
  padding: 12px 14px;
  border-radius: 14px;
  color: #334155;
  font-weight: 700;
  transition: background-color 0.16s ease, color 0.16s ease;
}

.nav-link:hover {
  background: #f8fafc;
}

.nav-link.router-link-active {
  background: linear-gradient(135deg, #eff6ff, #dbeafe);
  color: #1d4ed8;
  border: 1px solid #dbeafe;
}

.content-shell {
  min-width: 0;
  display: flex;
  flex-direction: column;
}

.topbar {
  padding: 24px 26px 0;
}

.topbar-kicker {
  margin: 0 0 6px;
  color: #2563eb;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  font-size: 0.74rem;
  font-weight: 800;
}

.topbar-title {
  margin: 0;
  font-size: clamp(1.6rem, 2.8vw, 2.2rem);
  line-height: 1.05;
  letter-spacing: -0.03em;
}

.page-container {
  padding: 22px 26px 28px;
}

@media (max-width: 980px) {
  .app-shell {
    grid-template-columns: 1fr;
  }

  .sidebar {
    border-right: none;
    border-bottom: 1px solid rgba(226, 232, 240, 0.9);
  }

  .nav-links {
    flex-direction: row;
    flex-wrap: wrap;
  }
}

@media (max-width: 768px) {
  .topbar,
  .page-container {
    padding-left: 14px;
    padding-right: 14px;
  }
}
</style>

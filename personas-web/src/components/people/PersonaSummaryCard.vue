<script setup>
defineProps({
  persona: {
    type: Object,
    required: true
  }
})
</script>

<template>
  <section class="profile-card">
    <div class="avatar">{{ (persona.nombre?.[0] || 'P').toUpperCase() }}</div>

    <div class="profile-content">
      <div class="profile-top">
        <div>
          <h3>{{ persona.nombre }} {{ persona.apellidos }}</h3>
          <p class="profile-email">{{ persona.correo }}</p>
        </div>

        <span
          class="status-badge"
          :class="persona.activo ? 'status-badge--active' : 'status-badge--inactive'"
        >
          {{ persona.activo ? 'Activo' : 'Inactivo' }}
        </span>
      </div>

      <div class="meta-grid">
        <div>
          <span class="meta-label">Identificación</span>
          <p>{{ persona.identificacion }}</p>
        </div>

        <div>
          <span class="meta-label">Teléfono</span>
          <p>{{ persona.telefono }}</p>
        </div>

        <div>
          <span class="meta-label">Fecha de nacimiento</span>
          <p>{{ persona.fechaNacimiento?.slice(0, 10) || '-' }}</p>
        </div>
      </div>
    </div>
  </section>
</template>

<style scoped>
.profile-card {
  display: flex;
  gap: 16px;
  align-items: flex-start;
  padding: 18px;
  border-radius: 20px;
  background: linear-gradient(135deg, rgba(239, 246, 255, 0.95), rgba(255, 255, 255, 0.94));
  border: 1px solid #dbeafe;
}

.avatar {
  width: 58px;
  height: 58px;
  border-radius: 18px;
  display: grid;
  place-items: center;
  color: #fff;
  font-size: 1.25rem;
  font-weight: 900;
  background: linear-gradient(135deg, #2563eb, #0ea5e9);
  box-shadow: 0 12px 22px rgba(37, 99, 235, 0.22);
  flex-shrink: 0;
}

.profile-content {
  flex: 1;
}

.profile-top {
  display: flex;
  justify-content: space-between;
  gap: 12px;
  align-items: start;
  flex-wrap: wrap;
  margin-bottom: 12px;
}

.profile-top h3 {
  margin: 0;
  font-size: 1.2rem;
}

.profile-email {
  margin: 6px 0 0;
  color: #64748b;
}

.meta-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 14px;
}

.meta-label {
  font-size: 0.82rem;
  color: #64748b;
  font-weight: 700;
}

.meta-grid p {
  margin: 6px 0 0;
  font-weight: 600;
  color: #0f172a;
}

.status-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 92px;
  padding: 6px 10px;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 900;
}

.status-badge--active {
  background: #dcfce7;
  color: #166534;
}

.status-badge--inactive {
  background: #fee2e2;
  color: #991b1b;
}

@media (max-width: 900px) {
  .profile-card {
    flex-direction: column;
  }

  .meta-grid {
    grid-template-columns: 1fr;
  }
}
</style>

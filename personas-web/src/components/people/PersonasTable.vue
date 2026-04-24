<script setup>
import BaseButton from '../ui/BaseButton.vue'

defineProps({
  personas: {
    type: Array,
    default: () => []
  },
  total: {
    type: Number,
    default: 0
  },
  page: {
    type: Number,
    default: 1
  },
  totalPages: {
    type: Number,
    default: 1
  },
  search: {
    type: String,
    default: ''
  },
  loading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits([
  'update:search',
  'search',
  'clear',
  'create',
  'detail',
  'edit',
  'toggle',
  'previous',
  'next'
])
</script>

<template>
  <div>
    <div class="table-toolbar">
      <div class="table-toolbar-left">
        <input
          :value="search"
          class="search-input"
          placeholder="Buscar por identificación, nombre o apellidos"
          @input="emit('update:search', $event.target.value)"
          @keyup.enter="emit('search')"
        />

        <BaseButton variant="secondary" size="sm" @click="emit('search')">
          Buscar
        </BaseButton>

        <BaseButton variant="ghost" size="sm" @click="emit('clear')">
          Limpiar
        </BaseButton>
      </div>

      <BaseButton variant="primary" size="sm" @click="emit('create')">
        Nueva persona
      </BaseButton>
    </div>

    <div class="table-shell">
      <div class="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>Identificación</th>
              <th>Nombre</th>
              <th>Correo</th>
              <th>Teléfono</th>
              <th>Estado</th>
              <th class="actions-col">Acciones</th>
            </tr>
          </thead>

          <tbody>
            <tr v-if="loading">
              <td colspan="6" class="empty-row">Cargando personas...</td>
            </tr>

            <tr v-else-if="personas.length === 0">
              <td colspan="6" class="empty-row">No hay resultados.</td>
            </tr>

            <tr v-for="persona in personas" :key="persona.personaId">
              <td>{{ persona.identificacion }}</td>
              <td class="strong-cell">{{ persona.nombreCompleto }}</td>
              <td>{{ persona.correo }}</td>
              <td>{{ persona.telefono }}</td>
              <td>
                <span
                  class="status-badge"
                  :class="persona.activo ? 'status-badge--active' : 'status-badge--inactive'"
                >
                  {{ persona.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td>
                <div class="actions">
                  <BaseButton variant="ghost" size="xs" @click="emit('detail', persona.personaId)">
                    Ver
                  </BaseButton>

                  <BaseButton variant="accent" size="xs" @click="emit('edit', persona.personaId)">
                    Editar
                  </BaseButton>

                  <BaseButton
                    :variant="persona.activo ? 'danger' : 'success'"
                    size="xs"
                    @click="emit('toggle', persona)"
                  >
                    {{ persona.activo ? 'Inactivar' : 'Activar' }}
                  </BaseButton>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="table-footer">
      <div class="summary-chip">
        Total registros: <strong>{{ total }}</strong>
      </div>

      <div class="pagination">
        <BaseButton variant="ghost" size="sm" :disabled="page <= 1" @click="emit('previous')">
          Anterior
        </BaseButton>

        <span class="pagination-pill">Página {{ page }} de {{ totalPages }}</span>

        <BaseButton variant="ghost" size="sm" :disabled="page >= totalPages" @click="emit('next')">
          Siguiente
        </BaseButton>
      </div>
    </div>
  </div>
</template>

<style scoped>
.table-toolbar {
  display: flex;
  justify-content: space-between;
  gap: 12px;
  align-items: center;
  flex-wrap: wrap;
  margin-bottom: 16px;
}

.table-toolbar-left {
  display: flex;
  gap: 10px;
  align-items: center;
  flex-wrap: wrap;
  flex: 1;
}

.search-input {
  flex: 1;
  min-width: 260px;
  height: 40px;
  padding: 0 13px;
  border: 1px solid #cbd5e1;
  border-radius: 12px;
  outline: none;
  font-size: 0.92rem;
  background: white;
}

.search-input:focus {
  border-color: #3b82f6;
  box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.12);
}

.table-shell {
  overflow: hidden;
  border: 1px solid #e2e8f0;
  border-radius: 18px;
  background: white;
}

.table-wrapper {
  overflow-x: auto;
}

table {
  width: 100%;
  min-width: 900px;
  border-collapse: collapse;
}

thead {
  background: #f8fafc;
}

th,
 td {
  padding: 13px;
  text-align: left;
  border-bottom: 1px solid #eef2f7;
}

th {
  font-size: 0.78rem;
  font-weight: 900;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: #64748b;
}

tbody tr:hover {
  background: #fbfdff;
}

.strong-cell {
  font-weight: 700;
  color: #0f172a;
}

.actions-col {
  min-width: 180px;
}

.actions {
  display: flex;
  gap: 6px;
  flex-wrap: wrap;
}

.status-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 82px;
  padding: 5px 9px;
  border-radius: 999px;
  font-size: 0.74rem;
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

.empty-row {
  text-align: center;
  color: #64748b;
  padding: 22px;
}

.table-footer {
  margin-top: 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}

.summary-chip,
.pagination-pill {
  padding: 9px 12px;
  border-radius: 999px;
  border: 1px solid #e2e8f0;
  background: #f8fafc;
  color: #334155;
  font-size: 0.84rem;
  font-weight: 700;
}

.pagination {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
}
</style>

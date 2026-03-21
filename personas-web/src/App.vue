<script setup>
import { onMounted, reactive, ref } from 'vue'
import { api } from './services/api'

const personas = ref([])
const total = ref(0)
const page = ref(1)
const pageSize = ref(10)
const search = ref('')
const message = ref('')
const selected = ref(null)

const form = reactive({
  personaId: 0,
  identificacion: '',
  nombre: '',
  apellidos: '',
  fechaNacimiento: '',
  correo: '',
  telefono: '',
  activo: true
})

const direccionForm = reactive({
  provincia: '',
  canton: '',
  distrito: '',
  direccionExacta: '',
  esPrincipal: false
})

function clearMessage() {
  message.value = ''
}

function resetForm() {
  form.personaId = 0
  form.identificacion = ''
  form.nombre = ''
  form.apellidos = ''
  form.fechaNacimiento = ''
  form.correo = ''
  form.telefono = ''
  form.activo = true
}

function resetDireccionForm() {
  direccionForm.provincia = ''
  direccionForm.canton = ''
  direccionForm.distrito = ''
  direccionForm.direccionExacta = ''
  direccionForm.esPrincipal = false
}

function validatePersona() {
  if (!form.identificacion.trim()) return 'La identificación es requerida.'
  if (!form.nombre.trim()) return 'El nombre es requerido.'
  if (!form.apellidos.trim()) return 'Los apellidos son requeridos.'
  if (!form.correo.trim()) return 'El correo es requerido.'
  if (!form.telefono.trim()) return 'El teléfono es requerido.'
  if (!form.fechaNacimiento) return 'La fecha de nacimiento es requerida.'

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(form.correo.trim())) return 'El correo no tiene un formato válido.'

  return ''
}

function validateDireccion(direccion) {
  if (!direccion.provincia.trim()) return 'La provincia es requerida.'
  if (!direccion.canton.trim()) return 'El cantón es requerido.'
  if (!direccion.distrito.trim()) return 'El distrito es requerido.'
  if (!direccion.direccionExacta.trim()) return 'La dirección exacta es requerida.'
  return ''
}

async function loadPersonas() {
  clearMessage()

  try {
    const data = await api.getPersonas(search.value, page.value, pageSize.value)
    personas.value = data.items || []
    total.value = data.total || 0
  } catch (e) {
    message.value = e.message
  }
}

async function savePersona() {
  clearMessage()

  const validationError = validatePersona()
  if (validationError) {
    message.value = validationError
    return
  }

  try {
    const payload = {
      identificacion: form.identificacion.trim(),
      nombre: form.nombre.trim(),
      apellidos: form.apellidos.trim(),
      fechaNacimiento: form.fechaNacimiento,
      correo: form.correo.trim(),
      telefono: form.telefono.trim(),
      activo: form.activo
    }

    if (form.personaId) {
      await api.updatePersona(form.personaId, payload)
      message.value = 'Persona actualizada correctamente.'
      selected.value = await api.getPersona(form.personaId)
    } else {
      const created = await api.createPersona(payload)
      message.value = 'Persona creada correctamente.'
      selected.value = created
    }

    resetForm()
    await loadPersonas()
  } catch (e) {
    message.value = e.message
  }
}

async function editPersona(id) {
  clearMessage()

  try {
    const data = await api.getPersona(id)
    selected.value = data

    form.personaId = data.personaId
    form.identificacion = data.identificacion || ''
    form.nombre = data.nombre || ''
    form.apellidos = data.apellidos || ''
    form.fechaNacimiento = data.fechaNacimiento ? data.fechaNacimiento.slice(0, 10) : ''
    form.correo = data.correo || ''
    form.telefono = data.telefono || ''
    form.activo = data.activo
  } catch (e) {
    message.value = e.message
  }
}

async function verDetalle(id) {
  clearMessage()

  try {
    selected.value = await api.getPersona(id)
  } catch (e) {
    message.value = e.message
  }
}

async function toggleActivo(persona) {
  clearMessage()

  try {
    await api.setActivo(persona.personaId, !persona.activo)
    message.value = 'Estado actualizado correctamente.'
    await loadPersonas()

    if (selected.value && selected.value.personaId === persona.personaId) {
      selected.value = await api.getPersona(persona.personaId)
    }
  } catch (e) {
    message.value = e.message
  }
}

async function saveDireccion() {
  clearMessage()

  if (!selected.value) return

  const validationError = validateDireccion(direccionForm)
  if (validationError) {
    message.value = validationError
    return
  }

  try {
    await api.addDireccion(selected.value.personaId, {
      provincia: direccionForm.provincia.trim(),
      canton: direccionForm.canton.trim(),
      distrito: direccionForm.distrito.trim(),
      direccionExacta: direccionForm.direccionExacta.trim(),
      esPrincipal: direccionForm.esPrincipal
    })

    message.value = 'Dirección agregada correctamente.'
    resetDireccionForm()
    selected.value = await api.getPersona(selected.value.personaId)
    await loadPersonas()
  } catch (e) {
    message.value = e.message
  }
}

async function updateDireccion(direccion) {
  clearMessage()

  const validationError = validateDireccion(direccion)
  if (validationError) {
    message.value = validationError
    return
  }

  try {
    await api.updateDireccion(direccion.direccionId, {
      provincia: direccion.provincia.trim(),
      canton: direccion.canton.trim(),
      distrito: direccion.distrito.trim(),
      direccionExacta: direccion.direccionExacta.trim(),
      esPrincipal: direccion.esPrincipal
    })

    selected.value = await api.getPersona(selected.value.personaId)
    await loadPersonas()
    message.value = 'Dirección actualizada correctamente.'
  } catch (e) {
    message.value = e.message
  }
}

async function deleteDireccion(id) {
  clearMessage()

  try {
    await api.deleteDireccion(id)
    message.value = 'Dirección eliminada correctamente.'
    selected.value = await api.getPersona(selected.value.personaId)
    await loadPersonas()
  } catch (e) {
    message.value = e.message
  }
}

function previousPage() {
  if (page.value > 1) {
    page.value--
    loadPersonas()
  }
}

function nextPage() {
  if (page.value * pageSize.value < total.value) {
    page.value++
    loadPersonas()
  }
}

onMounted(loadPersonas)
</script>

<template>
  <main class="page">
    <h1>Gestión de Personas</h1>
    <p class="message" v-if="message">{{ message }}</p>

    <section class="card">
      <h2>Listado de personas</h2>

      <div class="row">
        <input
          v-model="search"
          placeholder="Buscar por identificación, nombre o apellidos"
        />
        <button @click="page = 1; loadPersonas()">Buscar</button>
        <button @click="search = ''; page = 1; loadPersonas()">Limpiar búsqueda</button>
      </div>

      <table>
        <thead>
          <tr>
            <th>Identificación</th>
            <th>Nombre</th>
            <th>Correo</th>
            <th>Teléfono</th>
            <th>Activo</th>
            <th>Acciones</th>
          </tr>
        </thead>

        <tbody>
          <tr v-if="personas.length === 0">
            <td colspan="6">No hay resultados.</td>
          </tr>

          <tr v-for="persona in personas" :key="persona.personaId">
            <td>{{ persona.identificacion }}</td>
            <td>{{ persona.nombreCompleto }}</td>
            <td>{{ persona.correo }}</td>
            <td>{{ persona.telefono }}</td>
            <td>{{ persona.activo ? 'Sí' : 'No' }}</td>
            <td class="actions">
              <button @click="verDetalle(persona.personaId)">Detalle</button>
              <button @click="editPersona(persona.personaId)">Editar</button>
              <button @click="toggleActivo(persona)">
                {{ persona.activo ? 'Inactivar' : 'Activar' }}
              </button>
            </td>
          </tr>
        </tbody>
      </table>

      <div class="row pagination">
        <button :disabled="page <= 1" @click="previousPage">Anterior</button>
        <span>Página {{ page }}</span>
        <button :disabled="page * pageSize >= total" @click="nextPage">Siguiente</button>
        <span>Total registros: {{ total }}</span>
      </div>
    </section>

    <section class="card">
      <h2>{{ form.personaId ? 'Editar persona' : 'Crear persona' }}</h2>

      <div class="grid">
        <input v-model="form.identificacion" placeholder="Identificación" />
        <input v-model="form.nombre" placeholder="Nombre" />
        <input v-model="form.apellidos" placeholder="Apellidos" />
        <input v-model="form.fechaNacimiento" type="date" />
        <input v-model="form.correo" placeholder="Correo" />
        <input v-model="form.telefono" placeholder="Teléfono" />
      </div>

      <label class="checkbox-row">
        <input v-model="form.activo" type="checkbox" />
        Activo
      </label>

      <div class="row">
        <button @click="savePersona">{{ form.personaId ? 'Actualizar' : 'Guardar' }}</button>
        <button @click="resetForm">Limpiar</button>
      </div>
    </section>

    <section class="card" v-if="selected">
      <h2>Detalle de persona</h2>
      <p><strong>{{ selected.nombre }} {{ selected.apellidos }}</strong></p>
      <p>Identificación: {{ selected.identificacion }}</p>
      <p>Correo: {{ selected.correo }}</p>
      <p>Teléfono: {{ selected.telefono }}</p>
      <p>Activo: {{ selected.activo ? 'Sí' : 'No' }}</p>

      <h3>Nueva dirección</h3>
      <div class="grid">
        <input v-model="direccionForm.provincia" placeholder="Provincia" />
        <input v-model="direccionForm.canton" placeholder="Cantón" />
        <input v-model="direccionForm.distrito" placeholder="Distrito" />
        <input v-model="direccionForm.direccionExacta" placeholder="Dirección exacta" />
      </div>

      <label class="checkbox-row">
        <input v-model="direccionForm.esPrincipal" type="checkbox" />
        Principal
      </label>

      <div class="row">
        <button @click="saveDireccion">Agregar dirección</button>
      </div>

      <h3>Direcciones</h3>

      <div v-if="!selected.direcciones || selected.direcciones.length === 0">
        No hay direcciones registradas.
      </div>

      <div
        v-for="dir in selected.direcciones"
        :key="dir.direccionId"
        class="address"
      >
        <div class="grid">
          <input v-model="dir.provincia" />
          <input v-model="dir.canton" />
          <input v-model="dir.distrito" />
          <input v-model="dir.direccionExacta" />
        </div>

        <label class="checkbox-row">
          <input v-model="dir.esPrincipal" type="checkbox" />
          Principal
        </label>

        <div class="row">
          <button @click="updateDireccion(dir)">Guardar cambios</button>
          <button @click="deleteDireccion(dir.direccionId)">Eliminar</button>
        </div>
      </div>
    </section>
  </main>
</template>

<style>
body {
  font-family: Arial, sans-serif;
  background: #f5f6fa;
  margin: 0;
}

.page {
  max-width: 1100px;
  margin: 0 auto;
  padding: 24px;
}

.card {
  background: white;
  border-radius: 12px;
  padding: 16px;
  margin-bottom: 16px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, .06);
}

.grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 10px;
  margin-bottom: 12px;
}

.row {
  display: flex;
  gap: 10px;
  margin: 10px 0;
  flex-wrap: wrap;
  align-items: center;
}

.checkbox-row {
  display: flex;
  gap: 8px;
  align-items: center;
  margin: 10px 0;
}

input {
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 8px;
}

button {
  padding: 10px 14px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  background: #2563eb;
  color: white;
}

button:disabled {
  background: #94a3b8;
  cursor: not-allowed;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 12px;
}

th, td {
  text-align: left;
  padding: 10px;
  border-bottom: 1px solid #eee;
  vertical-align: top;
}

.actions {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.message {
  color: #b91c1c;
  font-weight: 600;
}

.address {
  padding: 12px;
  border: 1px solid #eee;
  border-radius: 10px;
  margin-bottom: 10px;
}

.pagination {
  justify-content: space-between;
}
</style>

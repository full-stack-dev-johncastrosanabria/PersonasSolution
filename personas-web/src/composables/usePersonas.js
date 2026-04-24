import { computed, reactive, ref } from 'vue'
import { api } from '../services/api'

export const DEFAULT_PAGE_SIZE = 10

export function createPersonaForm() {
  return {
    personaId: 0,
    identificacion: '',
    nombre: '',
    apellidos: '',
    fechaNacimiento: '',
    correo: '',
    telefono: '',
    activo: true
  }
}

export function createDireccionForm() {
  return {
    provincia: '',
    canton: '',
    distrito: '',
    direccionExacta: '',
    esPrincipal: false
  }
}

export function sanitize(value) {
  return typeof value === 'string' ? value.trim() : value
}

export function getErrorMessage(error) {
  return error?.response?.data?.message || error?.message || 'Ocurrió un error inesperado.'
}

export function fillPersonaForm(target, persona) {
  Object.assign(target, {
    personaId: persona.personaId ?? 0,
    identificacion: persona.identificacion ?? '',
    nombre: persona.nombre ?? '',
    apellidos: persona.apellidos ?? '',
    fechaNacimiento: persona.fechaNacimiento ? persona.fechaNacimiento.slice(0, 10) : '',
    correo: persona.correo ?? '',
    telefono: persona.telefono ?? '',
    activo: Boolean(persona.activo)
  })
}

export function buildPersonaPayload(form) {
  return {
    identificacion: sanitize(form.identificacion),
    nombre: sanitize(form.nombre),
    apellidos: sanitize(form.apellidos),
    fechaNacimiento: form.fechaNacimiento,
    correo: sanitize(form.correo),
    telefono: sanitize(form.telefono),
    activo: Boolean(form.activo)
  }
}

export function buildDireccionPayload(form) {
  return {
    provincia: sanitize(form.provincia),
    canton: sanitize(form.canton),
    distrito: sanitize(form.distrito),
    direccionExacta: sanitize(form.direccionExacta),
    esPrincipal: Boolean(form.esPrincipal)
  }
}

export function validatePersona(form) {
  if (!sanitize(form.identificacion)) return 'La identificación es requerida.'
  if (!sanitize(form.nombre)) return 'El nombre es requerido.'
  if (!sanitize(form.apellidos)) return 'Los apellidos son requeridos.'
  if (!sanitize(form.correo)) return 'El correo es requerido.'
  if (!sanitize(form.telefono)) return 'El teléfono es requerido.'
  if (!form.fechaNacimiento) return 'La fecha de nacimiento es requerida.'

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  if (!emailRegex.test(sanitize(form.correo))) {
    return 'El correo no tiene un formato válido.'
  }

  return ''
}

export function validateDireccion(form) {
  if (!sanitize(form.provincia)) return 'La provincia es requerida.'
  if (!sanitize(form.canton)) return 'El cantón es requerido.'
  if (!sanitize(form.distrito)) return 'El distrito es requerido.'
  if (!sanitize(form.direccionExacta)) return 'La dirección exacta es requerida.'
  return ''
}

export function usePersonasList() {
  const personas = ref([])
  const total = ref(0)
  const page = ref(1)
  const pageSize = ref(DEFAULT_PAGE_SIZE)
  const search = ref('')
  const loading = ref(false)
  const notification = reactive({
    type: '',
    text: ''
  })

  const totalPages = computed(() => Math.max(1, Math.ceil(total.value / pageSize.value)))

  function setNotification(type, text) {
    notification.type = type
    notification.text = text
  }

  function clearNotification() {
    notification.type = ''
    notification.text = ''
  }

  async function loadPersonas() {
    clearNotification()
    loading.value = true

    try {
      const data = await api.getPersonas(sanitize(search.value), page.value, pageSize.value)
      personas.value = data?.items ?? []
      total.value = data?.total ?? 0
    } catch (error) {
      setNotification('error', getErrorMessage(error))
    } finally {
      loading.value = false
    }
  }

  async function handleSearch() {
    page.value = 1
    await loadPersonas()
  }

  async function clearSearch() {
    search.value = ''
    page.value = 1
    await loadPersonas()
  }

  async function previousPage() {
    if (page.value <= 1) return
    page.value -= 1
    await loadPersonas()
  }

  async function nextPage() {
    if (page.value >= totalPages.value) return
    page.value += 1
    await loadPersonas()
  }

  async function toggleActivo(persona) {
    clearNotification()

    try {
      await api.setActivo(persona.personaId, !persona.activo)
      setNotification('success', 'Estado actualizado correctamente.')
      await loadPersonas()
    } catch (error) {
      setNotification('error', getErrorMessage(error))
    }
  }

  return {
    personas,
    total,
    page,
    pageSize,
    search,
    loading,
    notification,
    totalPages,
    loadPersonas,
    handleSearch,
    clearSearch,
    previousPage,
    nextPage,
    toggleActivo
  }
}

export function usePersonaForm() {
  const form = reactive(createPersonaForm())
  const saving = ref(false)
  const notification = reactive({
    type: '',
    text: ''
  })

  function setNotification(type, text) {
    notification.type = type
    notification.text = text
  }

  function clearNotification() {
    notification.type = ''
    notification.text = ''
  }

  function resetForm() {
    Object.assign(form, createPersonaForm())
  }

  async function loadPersona(id) {
    clearNotification()

    try {
      const persona = await api.getPersona(id)
      fillPersonaForm(form, persona)
      return persona
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      throw error
    }
  }

  async function createPersona() {
    clearNotification()
    const validationMessage = validatePersona(form)

    if (validationMessage) {
      setNotification('error', validationMessage)
      return null
    }

    saving.value = true

    try {
      const created = await api.createPersona(buildPersonaPayload(form))
      setNotification('success', 'Persona creada correctamente.')
      return created
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      return null
    } finally {
      saving.value = false
    }
  }

  async function updatePersona() {
    clearNotification()
    const validationMessage = validatePersona(form)

    if (validationMessage) {
      setNotification('error', validationMessage)
      return null
    }

    saving.value = true

    try {
      await api.updatePersona(form.personaId, buildPersonaPayload(form))
      setNotification('success', 'Persona actualizada correctamente.')
      return form.personaId
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      return null
    } finally {
      saving.value = false
    }
  }

  return {
    form,
    saving,
    notification,
    resetForm,
    loadPersona,
    createPersona,
    updatePersona
  }
}

export function usePersonaDetail() {
  const selected = ref(null)
  const direccionForm = reactive(createDireccionForm())
  const loading = ref(false)
  const savingDireccion = ref(false)
  const notification = reactive({
    type: '',
    text: ''
  })

  function setNotification(type, text) {
    notification.type = type
    notification.text = text
  }

  function clearNotification() {
    notification.type = ''
    notification.text = ''
  }

  function resetDireccionForm() {
    Object.assign(direccionForm, createDireccionForm())
  }

  async function loadPersona(id) {
    clearNotification()
    loading.value = true

    try {
      selected.value = await api.getPersona(id)
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      throw error
    } finally {
      loading.value = false
    }
  }

  async function refreshPersona() {
    if (!selected.value?.personaId) return
    selected.value = await api.getPersona(selected.value.personaId)
  }

  async function saveDireccion() {
    clearNotification()
    const validationMessage = validateDireccion(direccionForm)

    if (validationMessage) {
      setNotification('error', validationMessage)
      return false
    }

    savingDireccion.value = true

    try {
      await api.addDireccion(selected.value.personaId, buildDireccionPayload(direccionForm))
      resetDireccionForm()
      await refreshPersona()
      setNotification('success', 'Dirección agregada correctamente.')
      return true
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      return false
    } finally {
      savingDireccion.value = false
    }
  }

  async function updateDireccion(direccion) {
    clearNotification()
    const validationMessage = validateDireccion(direccion)

    if (validationMessage) {
      setNotification('error', validationMessage)
      return false
    }

    try {
      await api.updateDireccion(direccion.direccionId, buildDireccionPayload(direccion))
      await refreshPersona()
      setNotification('success', 'Dirección actualizada correctamente.')
      return true
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      return false
    }
  }

  async function deleteDireccion(id) {
    clearNotification()

    try {
      await api.deleteDireccion(id)
      await refreshPersona()
      setNotification('success', 'Dirección eliminada correctamente.')
      return true
    } catch (error) {
      setNotification('error', getErrorMessage(error))
      return false
    }
  }

  return {
    selected,
    direccionForm,
    loading,
    savingDireccion,
    notification,
    loadPersona,
    saveDireccion,
    updateDireccion,
    deleteDireccion
  }
}

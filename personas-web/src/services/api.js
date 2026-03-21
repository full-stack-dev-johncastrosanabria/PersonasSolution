const API = 'http://localhost:58704/api'

async function request(url, options = {}) {
  const response = await fetch(`${API}${url}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options
  })

  if (!response.ok) {
    const errorBody = await response.json().catch(() => null)
    const errorMessage =
      errorBody?.message ||
      errorBody?.detail ||
      errorBody?.title ||
      'Error inesperado'

    throw new Error(errorMessage)
  }

  if (response.status === 204) return null
  return response.json()
}

export const api = {
  getPersonas(search = '', page = 1, pageSize = 10) {
    const params = new URLSearchParams({ search, page, pageSize })
    return request(`/personas?${params.toString()}`)
  },
  getPersona(id) {
    return request(`/personas/${id}`)
  },
  createPersona(payload) {
    return request('/personas', { method: 'POST', body: JSON.stringify(payload) })
  },
  updatePersona(id, payload) {
    return request(`/personas/${id}`, { method: 'PUT', body: JSON.stringify(payload) })
  },
  setActivo(id, activo) {
    return request(`/personas/${id}/activo`, { method: 'PATCH', body: JSON.stringify({ activo }) })
  },
  addDireccion(personaId, payload) {
    return request(`/personas/${personaId}/direcciones`, { method: 'POST', body: JSON.stringify(payload) })
  },
  updateDireccion(id, payload) {
    return request(`/direcciones/${id}`, { method: 'PUT', body: JSON.stringify(payload) })
  },
  deleteDireccion(id) {
    return request(`/direcciones/${id}`, { method: 'DELETE' })
  }
}

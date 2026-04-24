const API_BASE_URL = 'http://localhost:58704/api'

async function request(path, options = {}) {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers || {})
    },
    ...options
  })

  if (!response.ok) {
    let message = 'Ocurrió un error al procesar la solicitud.'

    try {
      const errorData = await response.json()
      message = errorData?.message || errorData?.title || message
    } catch {
      // no-op
    }

    const error = new Error(message)
    error.response = { data: { message } }
    throw error
  }

  if (response.status === 204) {
    return null
  }

  return response.json()
}

export const api = {
  getPersonas(search = '', page = 1, pageSize = 10) {
    const params = new URLSearchParams({
      search,
      page: String(page),
      pageSize: String(pageSize)
    })

    return request(`/personas?${params.toString()}`)
  },

  getPersona(id) {
    return request(`/personas/${id}`)
  },

  createPersona(payload) {
    return request('/personas', {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },

  updatePersona(id, payload) {
    return request(`/personas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(payload)
    })
  },

  setActivo(id, activo) {
    return request(`/personas/${id}/activo`, {
      method: 'PATCH',
      body: JSON.stringify({ activo })
    })
  },

  addDireccion(personaId, payload) {
    return request(`/personas/${personaId}/direcciones`, {
      method: 'POST',
      body: JSON.stringify(payload)
    })
  },

  updateDireccion(direccionId, payload) {
    return request(`/direcciones/${direccionId}`, {
      method: 'PUT',
      body: JSON.stringify(payload)
    })
  },

  deleteDireccion(direccionId) {
    return request(`/direcciones/${direccionId}`, {
      method: 'DELETE'
    })
  }
}

<script setup>
import BaseButton from '../ui/BaseButton.vue'
import BaseInput from '../ui/BaseInput.vue'

const props = defineProps({
  form: {
    type: Object,
    required: true
  },
  saving: {
    type: Boolean,
    default: false
  },
  submitLabel: {
    type: String,
    default: 'Guardar'
  },
  showCancel: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['submit', 'reset', 'cancel'])
</script>

<template>
  <form class="form-layout" @submit.prevent="emit('submit')">
    <div class="form-grid">
      <BaseInput v-model="form.identificacion" label="Identificación" placeholder="Ej. 1-1234-5678" />
      <BaseInput v-model="form.nombre" label="Nombre" placeholder="Nombre" />
      <BaseInput v-model="form.apellidos" label="Apellidos" placeholder="Apellidos" />
      <BaseInput v-model="form.fechaNacimiento" label="Fecha de nacimiento" type="date" />
      <BaseInput v-model="form.correo" label="Correo" placeholder="correo@ejemplo.com" />
      <BaseInput v-model="form.telefono" label="Teléfono" placeholder="8888-8888" />
    </div>

    <label class="switch-row">
      <input v-model="form.activo" type="checkbox" />
      <span>Persona activa</span>
    </label>

    <div class="form-actions">
      <BaseButton type="submit" variant="primary" size="sm" :disabled="saving">
        {{ saving ? 'Guardando...' : submitLabel }}
      </BaseButton>

      <BaseButton type="button" variant="ghost" size="sm" @click="emit('reset')">
        Limpiar
      </BaseButton>

      <BaseButton v-if="showCancel" type="button" variant="secondary" size="sm" @click="emit('cancel')">
        Volver
      </BaseButton>
    </div>
  </form>
</template>

<style scoped>
.form-layout {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 14px;
}

.switch-row {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 10px 12px;
  border-radius: 14px;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  color: #334155;
  font-weight: 600;
  width: fit-content;
}

.form-actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}
</style>

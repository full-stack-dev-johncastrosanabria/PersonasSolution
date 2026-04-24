<script setup>
import BaseButton from '../ui/BaseButton.vue'
import BaseInput from '../ui/BaseInput.vue'

defineProps({
  direccionForm: {
    type: Object,
    required: true
  },
  direcciones: {
    type: Array,
    default: () => []
  },
  savingDireccion: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['save-direccion', 'update-direccion', 'delete-direccion'])
</script>

<template>
  <div class="stack">
    <section class="block">
      <div class="block-header">
        <div>
          <h3>Nueva dirección</h3>
          <p>Agrega una dirección para esta persona.</p>
        </div>
      </div>

      <div class="grid">
        <BaseInput v-model="direccionForm.provincia" label="Provincia" />
        <BaseInput v-model="direccionForm.canton" label="Cantón" />
        <BaseInput v-model="direccionForm.distrito" label="Distrito" />
        <BaseInput v-model="direccionForm.direccionExacta" label="Dirección exacta" />
      </div>

      <label class="switch-row">
        <input v-model="direccionForm.esPrincipal" type="checkbox" />
        <span>Dirección principal</span>
      </label>

      <div class="actions">
        <BaseButton variant="primary" size="sm" :disabled="savingDireccion" @click="emit('save-direccion')">
          {{ savingDireccion ? 'Guardando...' : 'Agregar dirección' }}
        </BaseButton>
      </div>
    </section>

    <section class="block">
      <div class="block-header">
        <div>
          <h3>Direcciones registradas</h3>
          <p>Edita, marca una principal o elimina registros existentes.</p>
        </div>
      </div>

      <div v-if="!direcciones.length" class="empty-box">
        No hay direcciones registradas.
      </div>

      <article v-for="dir in direcciones" :key="dir.direccionId" class="address-card">
        <div class="address-head">
          <strong>Dirección #{{ dir.direccionId }}</strong>

          <span class="mini-badge" :class="dir.esPrincipal ? 'mini-badge--primary' : 'mini-badge--neutral'">
            {{ dir.esPrincipal ? 'Principal' : 'Secundaria' }}
          </span>
        </div>

        <div class="grid">
          <BaseInput v-model="dir.provincia" label="Provincia" />
          <BaseInput v-model="dir.canton" label="Cantón" />
          <BaseInput v-model="dir.distrito" label="Distrito" />
          <BaseInput v-model="dir.direccionExacta" label="Dirección exacta" />
        </div>

        <label class="switch-row">
          <input v-model="dir.esPrincipal" type="checkbox" />
          <span>Principal</span>
        </label>

        <div class="actions">
          <BaseButton variant="accent" size="sm" @click="emit('update-direccion', dir)">
            Guardar cambios
          </BaseButton>

          <BaseButton variant="danger" size="sm" @click="emit('delete-direccion', dir.direccionId)">
            Eliminar
          </BaseButton>
        </div>
      </article>
    </section>
  </div>
</template>

<style scoped>
.stack {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.block {
  padding: 18px;
  border-radius: 20px;
  border: 1px solid #e2e8f0;
  background: rgba(255, 255, 255, 0.82);
}

.block-header {
  margin-bottom: 14px;
}

.block-header h3 {
  margin: 0;
  font-size: 1rem;
}

.block-header p {
  margin: 6px 0 0;
  color: #64748b;
}

.grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 14px;
}

.switch-row {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  margin: 14px 0 0;
  padding: 10px 12px;
  border-radius: 14px;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  color: #334155;
  font-weight: 600;
}

.actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
  margin-top: 14px;
}

.address-card {
  margin-top: 14px;
  padding: 14px;
  border-radius: 16px;
  border: 1px solid #e2e8f0;
  background: white;
}

.address-head {
  display: flex;
  justify-content: space-between;
  gap: 12px;
  align-items: center;
  margin-bottom: 12px;
  flex-wrap: wrap;
}

.mini-badge {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 90px;
  padding: 6px 10px;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 800;
}

.mini-badge--primary {
  background: #dbeafe;
  color: #1d4ed8;
}

.mini-badge--neutral {
  background: #f1f5f9;
  color: #475569;
}

.empty-box {
  text-align: center;
  color: #64748b;
  padding: 20px;
  border-radius: 14px;
  border: 1px dashed #cbd5e1;
  background: #f8fafc;
}

@media (max-width: 768px) {
  .grid {
    grid-template-columns: 1fr;
  }
}
</style>

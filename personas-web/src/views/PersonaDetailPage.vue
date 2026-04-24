<script setup>
import { computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import DireccionesManager from '../components/people/DireccionesManager.vue'
import PersonaSummaryCard from '../components/people/PersonaSummaryCard.vue'
import BaseAlert from '../components/ui/BaseAlert.vue'
import BaseButton from '../components/ui/BaseButton.vue'
import BasePageHeader from '../components/ui/BasePageHeader.vue'
import BasePanel from '../components/ui/BasePanel.vue'
import { usePersonaDetail } from '../composables/usePersonas'

const route = useRoute()
const router = useRouter()

const {
  selected,
  direccionForm,
  loading,
  savingDireccion,
  notification,
  loadPersona,
  saveDireccion,
  updateDireccion,
  deleteDireccion
} = usePersonaDetail()

const pageSubtitle = computed(() => {
  if (!selected.value) {
    return 'Consulta la información principal y administra sus direcciones desde una página dedicada.'
  }

  return `Gestiona direcciones y consulta el detalle de ${selected.value.nombre} ${selected.value.apellidos}.`
})

function goToEdit() {
  router.push({ name: 'personas-edit', params: { id: route.params.id } })
}

function goToList() {
  router.push({ name: 'personas-list' })
}

onMounted(() => loadPersona(route.params.id))
</script>

<template>
  <div class="page-stack">
    <BasePageHeader
      kicker="Detalle"
      title="Detalle de persona"
      :subtitle="pageSubtitle"
    />

    <BaseAlert :type="notification.type" :text="notification.text" />

    <div v-if="loading" class="loading-box">
      Cargando información de la persona...
    </div>

    <template v-else-if="selected">
      <div class="page-actions">
        <BaseButton variant="accent" size="sm" @click="goToEdit">
          Editar persona
        </BaseButton>

        <BaseButton variant="ghost" size="sm" @click="goToList">
          Volver al listado
        </BaseButton>
      </div>

      <BasePanel>
        <PersonaSummaryCard :persona="selected" />
      </BasePanel>

      <DireccionesManager
        :direccion-form="direccionForm"
        :direcciones="selected.direcciones || []"
        :saving-direccion="savingDireccion"
        @save-direccion="saveDireccion"
        @update-direccion="updateDireccion"
        @delete-direccion="deleteDireccion"
      />
    </template>
  </div>
</template>

<style scoped>
.page-stack {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.page-actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.loading-box {
  padding: 18px;
  border-radius: 18px;
  border: 1px solid #e2e8f0;
  background: rgba(255, 255, 255, 0.82);
  color: #475569;
  font-weight: 700;
}
</style>

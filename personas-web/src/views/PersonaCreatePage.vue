<script setup>
import { useRouter } from 'vue-router'
import PersonaFormFields from '../components/people/PersonaFormFields.vue'
import BaseAlert from '../components/ui/BaseAlert.vue'
import BasePageHeader from '../components/ui/BasePageHeader.vue'
import BasePanel from '../components/ui/BasePanel.vue'
import { usePersonaForm } from '../composables/usePersonas'

const router = useRouter()

const {
  form,
  saving,
  notification,
  resetForm,
  createPersona
} = usePersonaForm()

async function handleSubmit() {
  const created = await createPersona()

  if (created?.personaId) {
    router.push({ name: 'personas-detail', params: { id: created.personaId } })
  }
}

function handleCancel() {
  router.push({ name: 'personas-list' })
}
</script>

<template>
  <div class="page-stack">
    <BasePageHeader
      kicker="Crear"
      title="Nueva persona"
      subtitle="Página dedicada solo al registro, sin mezclar listado ni detalle en la misma vista."
    />

    <BaseAlert :type="notification.type" :text="notification.text" />

    <BasePanel>
      <PersonaFormFields
        :form="form"
        :saving="saving"
        submit-label="Crear persona"
        :show-cancel="true"
        @submit="handleSubmit"
        @reset="resetForm"
        @cancel="handleCancel"
      />
    </BasePanel>
  </div>
</template>

<style scoped>
.page-stack {
  display: flex;
  flex-direction: column;
  gap: 18px;
}
</style>

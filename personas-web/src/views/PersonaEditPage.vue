<script setup>
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import PersonaFormFields from '../components/people/PersonaFormFields.vue'
import BaseAlert from '../components/ui/BaseAlert.vue'
import BasePageHeader from '../components/ui/BasePageHeader.vue'
import BasePanel from '../components/ui/BasePanel.vue'
import { usePersonaForm } from '../composables/usePersonas'

const route = useRoute()
const router = useRouter()

const {
  form,
  saving,
  notification,
  resetForm,
  loadPersona,
  updatePersona
} = usePersonaForm()

async function handleSubmit() {
  const updatedId = await updatePersona()

  if (updatedId) {
    router.push({ name: 'personas-detail', params: { id: updatedId } })
  }
}

function handleCancel() {
  router.push({ name: 'personas-detail', params: { id: route.params.id } })
}

onMounted(() => loadPersona(route.params.id))
</script>

<template>
  <div class="page-stack">
    <BasePageHeader
      kicker="Editar"
      title="Editar persona"
      subtitle="Pantalla enfocada solo en la edición de la información principal."
    />

    <BaseAlert :type="notification.type" :text="notification.text" />

    <BasePanel>
      <PersonaFormFields
        :form="form"
        :saving="saving"
        submit-label="Guardar cambios"
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

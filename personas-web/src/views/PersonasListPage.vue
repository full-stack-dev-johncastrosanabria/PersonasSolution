<script setup>
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import PersonasTable from '../components/people/PersonasTable.vue'
import BaseAlert from '../components/ui/BaseAlert.vue'
import BasePageHeader from '../components/ui/BasePageHeader.vue'
import BasePanel from '../components/ui/BasePanel.vue'
import { usePersonasList } from '../composables/usePersonas'

const router = useRouter()

const {
  personas,
  total,
  page,
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
} = usePersonasList()

function goToCreate() {
  router.push({ name: 'personas-create' })
}

function goToDetail(id) {
  router.push({ name: 'personas-detail', params: { id } })
}

function goToEdit(id) {
  router.push({ name: 'personas-edit', params: { id } })
}

onMounted(loadPersonas)
</script>

<template>
  <div class="page-stack">
    <BasePageHeader
      kicker="Listado"
      title="Personas registradas"
      subtitle="Vista principal enfocada solo en consulta, búsqueda y acciones rápidas."
    />

    <BaseAlert :type="notification.type" :text="notification.text" />

    <BasePanel>
      <PersonasTable
        v-model:search="search"
        :personas="personas"
        :total="total"
        :page="page"
        :total-pages="totalPages"
        :loading="loading"
        @search="handleSearch"
        @clear="clearSearch"
        @create="goToCreate"
        @detail="goToDetail"
        @edit="goToEdit"
        @toggle="toggleActivo"
        @previous="previousPage"
        @next="nextPage"
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

import { createRouter, createWebHistory } from 'vue-router'
import PersonasListPage from '../views/PersonasListPage.vue'
import PersonaCreatePage from '../views/PersonaCreatePage.vue'
import PersonaEditPage from '../views/PersonaEditPage.vue'
import PersonaDetailPage from '../views/PersonaDetailPage.vue'

const routes = [
  {
    path: '/',
    redirect: '/personas'
  },
  {
    path: '/personas',
    name: 'personas-list',
    component: PersonasListPage
  },
  {
    path: '/personas/nueva',
    name: 'personas-create',
    component: PersonaCreatePage
  },
  {
    path: '/personas/:id',
    name: 'personas-detail',
    component: PersonaDetailPage,
    props: true
  },
  {
    path: '/personas/:id/editar',
    name: 'personas-edit',
    component: PersonaEditPage,
    props: true
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router

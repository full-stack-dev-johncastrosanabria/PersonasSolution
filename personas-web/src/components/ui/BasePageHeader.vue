<script setup>
defineProps({
  kicker: {
    type: String,
    default: 'Página'
  },
  title: {
    type: String,
    default: ''
  },
  subtitle: {
    type: String,
    default: ''
  }
})
</script>

<template>
  <section class="page-header">
    <span class="page-header-kicker">{{ kicker }}</span>
    <h1 class="page-header-title">{{ title }}</h1>
    <p v-if="subtitle" class="page-header-subtitle">{{ subtitle }}</p>
  </section>
</template>

<style scoped>
.page-header {
  margin-bottom: 18px;
}

.page-header-kicker {
  display: inline-block;
  margin-bottom: 8px;
  padding: 6px 10px;
  border-radius: 999px;
  background: rgba(37, 99, 235, 0.1);
  color: #1d4ed8;
  font-size: 0.76rem;
  font-weight: 800;
  letter-spacing: 0.05em;
  text-transform: uppercase;
}

.page-header-title {
  margin: 0;
  font-size: clamp(1.6rem, 2.7vw, 2.2rem);
  line-height: 1.05;
  letter-spacing: -0.03em;
  font-weight: 900;
}

.page-header-subtitle {
  margin: 10px 0 0;
  color: #475569;
  font-size: 0.97rem;
}
</style>

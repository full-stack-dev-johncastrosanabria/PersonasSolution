<script setup>
defineProps({
  type: {
    type: String,
    default: 'success'
  },
  text: {
    type: String,
    default: ''
  }
})
</script>

<template>
  <div v-if="text" class="alert" :class="`alert--${type}`">
    <div class="alert-icon">{{ type === 'error' ? '!' : '✓' }}</div>
    <div>{{ text }}</div>
  </div>
</template>

<style scoped>
.alert {
  display: flex;
  align-items: center;
  gap: 12px;
  border-radius: 16px;
  padding: 13px 15px;
  border: 1px solid transparent;
  font-weight: 700;
}

.alert-icon {
  width: 26px;
  height: 26px;
  border-radius: 999px;
  display: grid;
  place-items: center;
  flex-shrink: 0;
  font-weight: 900;
}

.alert--success {
  color: #166534;
  background: #ecfdf5;
  border-color: #bbf7d0;
}

.alert--success .alert-icon {
  background: #dcfce7;
  color: #15803d;
}

.alert--error {
  color: #991b1b;
  background: #fef2f2;
  border-color: #fecaca;
}

.alert--error .alert-icon {
  background: #fee2e2;
  color: #b91c1c;
}
</style>

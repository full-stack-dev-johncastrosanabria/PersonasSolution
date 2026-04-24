<script setup>
defineProps({
  type: {
    type: String,
    default: 'button'
  },
  variant: {
    type: String,
    default: 'primary'
  },
  size: {
    type: String,
    default: 'sm'
  },
  disabled: {
    type: Boolean,
    default: false
  }
})
</script>

<template>
  <button
    :type="type"
    :disabled="disabled"
    class="btn"
    :class="[`btn--${variant}`, `btn--${size}`]"
  >
    <slot />
  </button>
</template>

<style scoped>
.btn {
  border: none;
  border-radius: 12px;
  cursor: pointer;
  font-weight: 800;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  white-space: nowrap;
  transition:
    transform 0.12s ease,
    background-color 0.16s ease,
    border-color 0.16s ease,
    color 0.16s ease,
    opacity 0.16s ease,
    box-shadow 0.16s ease;
}

.btn:hover:not(:disabled) {
  transform: translateY(-1px);
}

.btn:disabled {
  opacity: 0.55;
  cursor: not-allowed;
  box-shadow: none;
}

.btn--xs {
  height: 30px;
  padding: 0 10px;
  font-size: 0.76rem;
}

.btn--sm {
  height: 34px;
  padding: 0 12px;
  font-size: 0.83rem;
}

.btn--md {
  height: 40px;
  padding: 0 15px;
  font-size: 0.9rem;
}

.btn--primary {
  background: #2563eb;
  color: white;
  box-shadow: 0 10px 18px rgba(37, 99, 235, 0.18);
}

.btn--primary:hover:not(:disabled) {
  background: #1d4ed8;
}

.btn--secondary {
  background: #e2e8f0;
  color: #0f172a;
}

.btn--secondary:hover:not(:disabled) {
  background: #cbd5e1;
}

.btn--ghost {
  background: white;
  color: #334155;
  border: 1px solid #dbe3ee;
}

.btn--ghost:hover:not(:disabled) {
  background: #f8fafc;
}

.btn--accent {
  background: #eff6ff;
  color: #1d4ed8;
  border: 1px solid #dbeafe;
}

.btn--accent:hover:not(:disabled) {
  background: #dbeafe;
}

.btn--success {
  background: #f0fdf4;
  color: #166534;
  border: 1px solid #bbf7d0;
}

.btn--success:hover:not(:disabled) {
  background: #dcfce7;
}

.btn--danger {
  background: #fef2f2;
  color: #b91c1c;
  border: 1px solid #fecaca;
}

.btn--danger:hover:not(:disabled) {
  background: #fee2e2;
}
</style>

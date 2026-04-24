<template>
  <section class="panel">
    <slot />
  </section>
</template>

<style scoped>
.panel {
  background: rgba(255, 255, 255, 0.9);
  border: 1px solid #e2e8f0;
  border-radius: 22px;
  padding: 20px;
  box-shadow: 0 14px 35px rgba(15, 23, 42, 0.07);
}

@media (max-width: 768px) {
  .panel {
    padding: 16px;
  }
}
</style>

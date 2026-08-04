<template>
  <div v-if="account && account.id" class="fab-container position-fixed bottom-0 end-0 p-3 mb-3 me-3 d-flex flex-column gap-2">
    <button
      class="btn btn-success fab-btn shadow-lg rounded-pill px-3 py-2 d-flex align-items-center gap-2"
      @click="createKeep"
      title="Create New Keep"
    >
      <i class="mdi mdi-plus fs-4"></i>
      <span class="fw-bold">New Keep</span>
    </button>
    <button
      class="btn btn-primary fab-btn shadow-lg rounded-pill px-3 py-2 d-flex align-items-center gap-2"
      @click="createVault"
      title="Create New Vault"
    >
      <i class="mdi mdi-folder-plus fs-4"></i>
      <span class="fw-bold">New Vault</span>
    </button>
  </div>
</template>

<script>
import { computed } from 'vue';
import { AppState } from '../AppState';
import { Modal } from 'bootstrap';

export default {
  name: 'FloatingActionButtons',
  setup() {
    return {
      account: computed(() => AppState.account),
      createKeep() {
        Modal.getOrCreateInstance(document.getElementById('new-keep-modal')).show();
      },
      createVault() {
        Modal.getOrCreateInstance(document.getElementById('new-vault-modal')).show();
      }
    };
  }
};
</script>

<style scoped>
.fab-container {
  z-index: 1050;
}
.fab-btn {
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  font-size: 0.95rem;
  border: 2px solid rgba(255, 255, 255, 0.2);
}
.fab-btn:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.5) !important;
}
</style>

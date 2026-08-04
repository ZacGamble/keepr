<template>
  <nav
    class="
      navbar navbar-expand-lg
      bg-secondary
      px-3
      justify-content-between
      d-flex
    "
  >
    <router-link class="navbar-brand d-flex" :to="{ name: 'Home' }">
      <div class="d-flex flex-column align-items-center">
        <h2>
          <em class="p-1 fw-bold fs-1 shade">Keepr</em>
        </h2>
      </div>
    </router-link>
    <button
      class="navbar-toggler"
      type="button"
      data-bs-toggle="collapse"
      data-bs-target="#navbarText"
      aria-controls="navbarText"
      aria-expanded="false"
      aria-label="Toggle navigation"
    >
      <span class="navbar-toggler-icon" />
    </button>
    <div class="collapse navbar-collapse" id="navbarText">
      <div class="d-flex gap-2 ms-auto me-3" v-if="account.id">
        <button class="btn btn-outline-light btn-sm" @click="createKeep" title="Create a new Keep">
          <i class="mdi mdi-plus"></i> New Keep
        </button>
        <button class="btn btn-outline-light btn-sm" @click="createVault" title="Create a new Vault">
          <i class="mdi mdi-plus-box"></i> New Vault
        </button>
      </div>
    </div>
    <Login />
  </nav>
</template>

<script>
import { computed } from 'vue';
import { AppState } from '../AppState';
import { Modal } from 'bootstrap';

export default {
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
  },
};
</script>

<style scoped>
.shade {
  text-shadow: 4px 4px 3px black;
}
.skew {
  transform: skew(-0.06turn, 18deg);
}
</style>

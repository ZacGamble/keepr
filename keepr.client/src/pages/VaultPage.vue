<template>
  <div class="d-flex justify-content-between">
    <div class="flex-column p-3">
      <h1 class="my-3">Vault Name</h1>
      <div class="my-3 fw-bold">Keeps in this vault:</div>
    </div>
    <div class="p-4">
      <button class="btn btn-outline-danger">Delete Vault</button>
    </div>
  </div>
  <hr />
  <div class="masonry-container">
    <div class="keep-container" v-for="k in vaultKeeps" :key="k.id">
      <div class="p-2" style="min-height: 20vh">
        <img
          @click="openVaultKeepModal(k)"
          :src="k.img"
          alt="keep image"
          class="img-fluid img-custom"
          :title="'Open ' + k.name + ' details'"
        />
        <div class="d-flex justify-content-between" style="height: 0px">
          <h2 class="keep-name">
            {{ k.name }}
          </h2>
          <img
            @click="goToProfile(k.creator.id)"
            :src="k.creator.picture"
            alt="profile image"
            class="thumbnail-img selectable"
            :title="'Visit profile of ' + k.creator.name"
          />
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed, onMounted } from '@vue/runtime-core';
import { useRoute } from 'vue-router'
import { vaultKeepsService } from '../services/VaultKeepsService.js'
import { logger } from '../utils/Logger';
import Pop from '../utils/Pop';
import { AppState } from '../AppState.js';
export default {
  setup() {
    const route = useRoute();
    onMounted(async () => {
      try {
        await vaultKeepsService.getKeepsByVaultId(route.params.id)
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      route,
      vaultKeeps: computed(() => AppState.vaultKeeps),
    }
  }
}
</script>


<style lang="scss" scoped>
</style>
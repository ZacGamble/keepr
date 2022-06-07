<template>
  <div class="d-flex justify-content-between">
    <div class="flex-column p-3">
      <h1 class="my-3">{{ activeVault?.name }}</h1>
      <div class="my-3 fw-bold">Keeps in this vault: {{ keepsInVault }}</div>
    </div>
    <div v-if="activeVault?.creatorId == account.id" class="p-4">
      <button class="btn btn-outline-danger" @click="deleteVault()">
        Delete Vault
      </button>
    </div>
  </div>

  <div class="masonry-container">
    <div class="keep-container" v-for="k in vaultKeeps" :key="k.id">
      <div class="vaultkeep">
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
  <VaultKeepModal />
</template>


<script>
import { computed, onMounted } from '@vue/runtime-core';
import { useRoute, useRouter } from 'vue-router'
import { vaultKeepsService } from '../services/VaultKeepsService.js'
import { logger } from '../utils/Logger';
import Pop from '../utils/Pop';
import { AppState } from '../AppState.js';
import { vaultsService } from '../services/VaultsService.js';
import { Modal } from 'bootstrap';
import { keepsService } from '../services/KeepsService.js';
import { AuthService } from '../services/AuthService.js';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    onMounted(async () => {
      try {
        // TODO Need to push home manual URL load-in aliens
        await vaultsService.getById(route.params.id)
        await vaultKeepsService.getKeepsByVaultId(route.params.id)
      } catch (error) {

        router.push({ name: 'Home' });
        logger.error(error)
        Pop.toast("You were not invited.", 'error')
      }
    })
    return {
      route,
      router,
      vaultKeeps: computed(() => AppState.vaultKeeps),
      keepsInVault: computed(() => AppState.vaultKeeps.length),
      activeVault: computed(() => AppState.activeVault),
      account: computed(() => AppState.account),

      async deleteVault() {
        try {
          if (await Pop.confirm()) {
            await vaultsService.deleteVault(route.params.id);
            router.push({ name: 'Home' });
            Pop.toast("Your vault has been permanantly destroyed.")
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async openVaultKeepModal(k) {
        AppState.activeKeep = k;
        Modal.getOrCreateInstance(document.getElementById('vaultkeep-modal')).show()
        await keepsService.incrementViews();
      },

    }
  }
}
</script>


<style lang="scss" scoped>
.img-custom {
  border-radius: 7%;
  box-shadow: 3px 3px 12px black;
  cursor: pointer;
}
// .img-custom:hover {
//   transform: scale(1);
// }
.keep-container {
  padding: 10px;
  -webkit-column-break-inside: avoid;
  page-break-inside: avoid;
  break-inside: avoid;
  animation-name: fadeInto;
  animation-duration: 5000ms;
}
@keyframes fadeInto {
  0% {
    opacity: 0;
  }
  40% {
    opacity: 0;
  }

  100% {
    opacity: 100;
  }
}
.keep-name {
  transform: translateY(-4em);
  margin-left: 0.8em;
  color: whitesmoke;
  text-shadow: 3px 3px 4px black;
}
.vaultkeep {
  max-height: 90%;
}
</style>
<template>
  <div class="d-flex justify-content-between">
    <div class="flex-column p-3">
      <h1 class="my-3">Vault Name</h1>
      <div class="my-3 fw-bold">Keeps in this vault:</div>
    </div>
    <div class="p-4">
      <button class="btn btn-outline-danger" @click="deleteVault()">
        Delete Vault
      </button>
    </div>
  </div>

  <div class="masonry-container">
    <div class="keep-container" v-for="k in vaultKeeps" :key="k.id">
      <div class="p-3" style="min-height: 20vh">
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

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    onMounted(async () => {
      try {
        // await this.checkAccess() TODO Need to push home manual URL load-in aliens
        await vaultKeepsService.getKeepsByVaultId(route.params.id)
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      route,
      router,
      vaultKeeps: computed(() => AppState.vaultKeeps),

      async deleteVault() {
        try {
          await vaultsService.deleteVault(route.params.id);
          router.push({ name: 'Home' });
          Pop.toast("Your vault has been permanantly destroyed.")
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      openVaultKeepModal(k) {
        AppState.activeKeep = k;
        Modal.getOrCreateInstance(document.getElementById('vaultkeep-modal')).show()
      },

      checkAccess() {
        const userId = AppState.account.id
        if (v.isPrivate && v.creatorId != userId) {
          router.push({ name: 'Home' })
          Pop.toast("Sorry, you weren't invited..")
          return
        }
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
  transform: translateY(-3em);
  margin-left: 0.8em;
  color: whitesmoke;
  text-shadow: 3px 3px 4px black;
}
</style>
<template>
  <div class="container">
    <div class="row">
      <div class="d-flex">
        <img :src="profile?.picture" alt="" class="img-clamp me-5 mt-5" />
        <div class="d-flex flex-column mt-5">
          <h1>{{ profile?.name }}</h1>
          <span>Vaults: {{ numberOfVaults }}</span>
          <span>Keeps: {{ numberOfKeeps }}</span>
        </div>
      </div>
      <hr class="mt-3" />
    </div>
    <h3 class="my-3" @click="createVault()">
      Vaults:
      <i
        v-if="route.params.id == account.id"
        class="fw-bold fs-1 text-primary plus p-1"
        >+</i
      >
    </h3>
    <div class="row">
      <div
        class="col-sm-6 col-md-4 col-lg-2 p-2"
        v-for="v in vaults"
        :key="v.id"
      >
        <!-- Vaults -->
        <div
          class="p-2 mt-2 vault rounded selectable"
          @click="goToVault(v)"
          style="background-size: cover"
          :style="`background-image: url(${v.img})`"
        >
          <div class="d-flex justify-content-between">
            <h5 class="">
              {{ v.name }} <br /><br />
              {{ v.description }}
            </h5>
          </div>
        </div>
      </div>
    </div>

    <div class="wrapper">
      <h3 class="my-3" @click="createKeep()">
        Keeps:
        <i
          v-if="route.params.id == account.id"
          class="fw-bold fs-1 text-primary p-1 plus"
          >+</i
        >
      </h3>
      <div class="masonry-container-profile">
        <div class="keep-container" v-for="k in keeps" :key="k.id">
          <!-- Keep Masonry -->
          <div class="p-2">
            <img
              @click="openKeepModal(k)"
              :src="k.img"
              alt="keep image"
              class="img-fluid img-custom"
              :title="'Open ' + k.name + ' details'"
            />
            <div class="d-flex justify-content-between" style="height: 0px">
              <h5 class="keep-name">
                {{ k.name }}
              </h5>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <NewKeepModal />
  <NewVaultModal />
</template>


<script>
import { computed } from '@vue/reactivity'
import { AppState } from '../AppState'
import { onMounted } from '@vue/runtime-core'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { profilesService } from '../services/ProfilesService'
import { keepsService } from '../services/KeepsService'
import { vaultsService } from '../services/VaultsService'
import { useRoute, useRouter } from 'vue-router'
import { Modal } from 'bootstrap'
export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    onMounted(async () => {
      try {
        await profilesService.getUserProfile(route.params.id);
        await keepsService.getUserKeeps(route.params.id);
        await vaultsService.getUserVaults(route.params.id);
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      route,
      account: computed(() => AppState.account),
      profile: computed(() => AppState.activeProfile),
      keeps: computed(() => AppState.keeps),
      vaults: computed(() => AppState.userVaults),
      numberOfKeeps: computed(() => AppState.keeps.length),
      numberOfVaults: computed(() => AppState.userVaults.length),

      async openKeepModal(k) {
        AppState.activeKeep = k;
        Modal.getOrCreateInstance(document.getElementById('keep-modal')).show()
        await keepsService.incrementViews();
      },

      async goToVault(v) {
        const userId = AppState.account.id
        if (v.isPrivate && v.creatorId != userId) {
          router.push({ name: 'Home' })
          Pop.toast("Sorry, you weren't invited..")
          return
        }
        AppState.activeVault = v
        router.push({ name: 'Vault', params: { id: v.id } })
      },

      createVault() {
        Modal.getOrCreateInstance(document.getElementById("new-vault-modal")).show()
      },

      createKeep() {
        Modal.getOrCreateInstance(document.getElementById('new-keep-modal')).show()
      }
    }
  }
}
</script>


<style lang="scss" scoped>
.img-clamp {
  height: 6em;
  width: 6em;
}

.plus {
  text-shadow: 2px 2px 2px black;
  cursor: pointer;
}

//#region Copied from homepage
.img-custom {
  border-radius: 7%;
  box-shadow: 3px 3px 12px black;
  cursor: pointer;
}
.img-custom:hover {
  transform: scale(1.02);
}
.keep-container {
  padding: 1px;
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
// #endregion

.vault {
  color: whitesmoke;
  text-shadow: 3px 3px 4px black;
  animation-name: fadeInto;
  animation-duration: 5000ms;
}

.masonry-container-profile {
  column-count: 6;
  column-gap: 0.5em;
}

@media only screen and (max-width: 1068px) {
  .masonry-container-profile {
    column-count: 4;
    column-gap: 0.5em;
  }
}

@media only screen and (max-width: 768px) {
  .masonry-container-profile {
    column-count: 2;
    column-gap: 0.5em;
  }
}

@media only screen and (max-width: 425px) {
  .masonry-container-profile {
    column-count: 1;
  }
}
</style>
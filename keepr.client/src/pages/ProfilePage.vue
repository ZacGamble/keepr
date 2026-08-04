<template>
  <div class="container py-4">
    <div class="row align-items-center mb-4">
      <div class="col-auto">
        <img :src="profile?.picture || account?.picture" alt="profile photo" class="rounded-circle shadow img-clamp" />
      </div>
      <div class="col">
        <h1 class="mb-1">{{ profile?.name || account?.name }}</h1>
        <div class="text-muted fs-5">
          <span class="me-4"><i class="mdi mdi-lock-outline"></i> <strong>{{ numberOfVaults }}</strong> Vaults</span>
          <span><i class="mdi mdi-image-multiple-outline"></i> <strong>{{ numberOfKeeps }}</strong> Keeps</span>
        </div>
      </div>
    </div>
    <hr class="mb-4" />

    <!-- Vaults Section -->
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="m-0 fs-3">Vaults</h2>
      <button
        v-if="isOwner"
        class="btn btn-primary shadow-sm"
        @click="createVault()"
      >
        <i class="mdi mdi-plus me-1"></i> Create Vault
      </button>
    </div>

    <div class="row mb-5">
      <div
        class="col-sm-6 col-md-4 col-lg-3 p-2"
        v-for="v in vaults"
        :key="v.id"
      >
        <div
          class="vault-card p-3 rounded-3 shadow-sm selectable text-white d-flex flex-column justify-content-between"
          @click="goToVault(v)"
          :style="`background-image: linear-gradient(rgba(0,0,0,0.3), rgba(0,0,0,0.7)), url(${v.img})`"
        >
          <div class="d-flex justify-content-between align-items-start">
            <h4 class="m-0 text-truncate font-weight-bold" :title="v.name">{{ v.name }}</h4>
            <i v-if="v.isPrivate" class="mdi mdi-lock text-warning fs-5" title="Private Vault"></i>
          </div>
          <p class="m-0 small text-light text-truncate-2">{{ v.description }}</p>
        </div>
      </div>
    </div>

    <!-- Keeps Section -->
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2 class="m-0 fs-3">Keeps</h2>
      <button
        v-if="isOwner"
        class="btn btn-success shadow-sm"
        @click="createKeep()"
      >
        <i class="mdi mdi-plus me-1"></i> Create Keep
      </button>
    </div>

    <div class="masonry-container-profile">
      <div class="keep-container mb-3" v-for="k in keeps" :key="k.id">
        <div class="keep-card position-relative overflow-hidden rounded-3 shadow-sm" @click="openKeepModal(k)">
          <img
            :src="k.img"
            alt="keep image"
            class="img-fluid w-100 keep-img"
            :title="'Open ' + k.name + ' details'"
          />
          <div class="keep-overlay d-flex justify-content-between align-items-end p-3">
            <h5 class="keep-name text-white m-0 text-truncate" :title="k.name">
              {{ k.name }}
            </h5>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed } from '@vue/reactivity'
import { AppState } from '../AppState'
import { watchEffect } from '@vue/runtime-core'
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

    watchEffect(async () => {
      try {
        const targetId = route.params.id !== 'undefined' && route.params.id ? route.params.id : AppState.account.id;
        if (targetId && route.name == "Profile") {
          await profilesService.getUserProfile(targetId);
          await keepsService.getUserKeeps(targetId);
          await vaultsService.getUserVaults(targetId);
        }
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    });

    return {
      route,
      account: computed(() => AppState.account),
      profile: computed(() => AppState.activeProfile),
      keeps: computed(() => AppState.keeps),
      vaults: computed(() => AppState.userVaults),
      numberOfKeeps: computed(() => AppState.keeps.length),
      numberOfVaults: computed(() => AppState.userVaults.length),
      isOwner: computed(() => {
        const profileId = route.params.id !== 'undefined' && route.params.id ? route.params.id : AppState.account.id;
        return AppState.account && profileId === AppState.account.id;
      }),

      async openKeepModal(k) {
        AppState.activeKeep = k;
        Modal.getOrCreateInstance(document.getElementById('keep-modal')).show();
        await keepsService.incrementViews();
      },

      async goToVault(v) {
        const userId = AppState.account.id;
        if (v.isPrivate && v.creatorId != userId) {
          router.push({ name: 'Home' });
          Pop.toast("Sorry, this vault is private.");
          return;
        }
        AppState.activeVault = v;
        router.push({ name: 'Vault', params: { id: v.id } });
      },

      createVault() {
        Modal.getOrCreateInstance(document.getElementById("new-vault-modal")).show();
      },

      createKeep() {
        Modal.getOrCreateInstance(document.getElementById('new-keep-modal')).show();
      }
    };
  }
};
</script>


<style lang="scss" scoped>
.img-clamp {
  height: 90px;
  width: 90px;
  object-fit: cover;
}

.vault-card {
  height: 140px;
  background-size: cover;
  background-position: center;
  transition: transform 0.2s ease, box-shadow 0.2s ease;

  &:hover {
    transform: translateY(-3px);
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.3) !important;
  }
}

.keep-card {
  cursor: pointer;
  transition: transform 0.2s ease-in-out;
  background-color: #2b2b2b;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3) !important;
  }
}

.keep-img {
  display: block;
  object-fit: cover;
  border-radius: 8px;
}

.keep-overlay {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  background: linear-gradient(to top, rgba(0, 0, 0, 0.85) 0%, rgba(0, 0, 0, 0.4) 60%, transparent 100%);
  border-bottom-left-radius: 8px;
  border-bottom-right-radius: 8px;

  .keep-name {
    font-size: 1.1rem;
    font-weight: 600;
    text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.8);
  }
}

.masonry-container-profile {
  column-count: 4;
  column-gap: 1em;
}

@media only screen and (max-width: 992px) {
  .masonry-container-profile {
    column-count: 3;
  }
}

@media only screen and (max-width: 768px) {
  .masonry-container-profile {
    column-count: 2;
  }
}

@media only screen and (max-width: 480px) {
  .masonry-container-profile {
    column-count: 1;
  }
}
</style>
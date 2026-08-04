<template>
  <div class="masonry-container p-3">
    <div class="keep-container mb-3" v-for="k in keeps" :key="k.id">
      <div class="keep-card position-relative overflow-hidden rounded-3 shadow-sm" @click="openKeepModal(k)">
        <img
          :src="k.img"
          alt="keep image"
          class="img-fluid w-100 keep-img"
          :title="'Open ' + k.name + ' details'"
        />
        <div class="keep-overlay d-flex justify-content-between align-items-end p-3">
          <h4 class="keep-name text-white m-0 text-truncate" :title="k.name">
            {{ k.name }}
          </h4>
          <img
            v-if="k.creator"
            @click.stop="goToProfile(k.creator.id)"
            :src="k.creator.picture"
            alt="profile image"
            class="profile-avatar rounded-circle border border-2 border-white shadow-sm"
            :title="'Visit profile of ' + k.creator.name"
          />
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed, onMounted } from '@vue/runtime-core'
import { logger } from '../utils/Logger';
import Pop from '../utils/Pop';
import { keepsService } from '../services/KeepsService.js'
import { profilesService } from '../services/ProfilesService.js'
import { vaultsService } from '../services/VaultsService.js'
import { AppState } from '../AppState';
import { Modal } from 'bootstrap';
import { useRouter } from 'vue-router';
export default {
  setup() {
    const router = useRouter();
    onMounted(async () => {
      try {
        await keepsService.getAll();
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      keeps: computed(() => AppState.keeps),

      async openKeepModal(k) {
        AppState.activeKeep = k;
        Modal.getOrCreateInstance(document.getElementById('keep-modal')).show()
        await keepsService.incrementViews();
      },

      async goToProfile(profileId) {
        try {
          if (!profileId) return;
          router.push({ name: 'Profile', params: { id: profileId } })
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      }
    }
  }
}
</script>


<style lang="scss" scoped>
.keep-card {
  cursor: pointer;
  transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
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
    font-size: 1.25rem;
    font-weight: 600;
    text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.8);
    max-width: 75%;
  }
}

.profile-avatar {
  width: 42px;
  height: 42px;
  object-fit: cover;
  flex-shrink: 0;
  cursor: pointer;
  transition: transform 0.15s ease;

  &:hover {
    transform: scale(1.1);
  }
}

.keep-container {
  break-inside: avoid;
  page-break-inside: avoid;
}
</style>
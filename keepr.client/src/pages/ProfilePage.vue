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
    </div>
    <h3>
      Vaults:
      <i class="fw-bold p-1">+</i>
    </h3>
    <div class="row">
      <div class="col-sm-6 col-md-4 col-lg-2" v-for="v in vaults" :key="v.id">
        <!-- Vaults -->
        <div class="p-2 mt-2 bg-dark text-light">
          <div class="d-flex justify-content-between">
            <h2 class="">
              {{ v.name }}
            </h2>
          </div>
        </div>
      </div>
    </div>

    <div class="wrapper">
      <h3>Keeps: <i class="fw-bold fs-3 p-1">+</i></h3>
      <div class="masonry-container">
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
              <h2 class="keep-name">
                {{ k.name }}
              </h2>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
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
import { useRoute } from 'vue-router'
export default {
  setup() {
    const route = useRoute();
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
      profile: computed(() => AppState.activeProfile),
      keeps: computed(() => AppState.userKeeps),
      vaults: computed(() => AppState.userVaults),
      numberOfKeeps: computed(() => AppState.userKeeps.length),
      numberOfVaults: computed(() => AppState.userVaults.length),
    }
  }
}
</script>


<style lang="scss" scoped>
.img-clamp {
  height: 6em;
  width: 6em;
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
}
.keep-name {
  transform: translateY(-3em);
  margin-left: 0.8em;
  color: whitesmoke;
  text-shadow: 3px 3px 4px black;
}

.masonry-container {
  column-count: 5;
  column-gap: 0.3em;
}

@media only screen and (max-width: 1068px) {
  .masonry-container {
    column-count: 4;
    column-gap: 0.5em;
  }
}
@media only screen and (max-width: 768px) {
  .masonry-container {
    column-count: 2;
    column-gap: 0.5em;
  }
}
@media only screen and (max-width: 575px) {
  .masonry-container {
    column-count: 1;
  }
  // #endregion
}
</style>
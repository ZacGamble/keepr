<template>
  <div class="masonry-container">
    <div class="keep-container" v-for="k in keeps" :key="k.id">
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
          <img
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
import { computed, onMounted } from '@vue/runtime-core'
import { logger } from '../utils/Logger';
import Pop from '../utils/Pop';
import { keepsService } from '../services/KeepsService.js'
import { AppState } from '../AppState';
import { Modal } from 'bootstrap';
export default {
  setup() {
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

      openKeepModal(k) {
        AppState.activeKeep = k;
        logger.log(AppState.activeKeep)
        Modal.getOrCreateInstance(document.getElementById('keep-modal')).show()
      }
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
.thumbnail-img {
  border-radius: 50%;
  height: 4em;
  transform: translateY(-6em);
  margin-right: 0.5em;
}

.masonry-container {
  column-count: 4;
  column-gap: 0.5em;
}

@media only screen and (max-width: 1068px) {
  .masonry-container {
    column-count: 3;
    column-gap: 0.5em;
  }
}
@media only screen and (max-width: 768px) {
  .masonry-container {
    column-count: 2;
    column-gap: 0.5em;
  }
}
@media only screen and (max-width: 425px) {
  .masonry-container {
    column-count: 1;
  }
}
</style>
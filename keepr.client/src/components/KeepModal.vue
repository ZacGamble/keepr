<template>
  <Modal id="keep-modal">
    <template #modal-body-slot>
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-6">
            <img
              :src="activeKeep?.img"
              alt="keep image"
              class="img-fluid rounded"
            />
          </div>
          <div class="col-md-6">
            <div class="row">
              <div class="d-flex justify-content-evenly">
                <div class="d-flex">
                  <i class="mdi mdi-eye mx-4 fs-2" />
                  <span class="fs-2">{{ activeKeep?.views }}</span>
                </div>
                <div class="d-flex">
                  <i class="mdi mdi-safe mx-4 fs-2" />
                  <span class="fs-2">{{ activeKeep?.kept }}</span>
                </div>
              </div>
            </div>
            <div class="row mt-4 mb-5">
              <div class="p-2 border-bottom border-dark">
                <h1 class="text-center">{{ activeKeep?.name }}</h1>
                <p class="mb-4">{{ activeKeep?.description }}</p>
              </div>
            </div>

            <!-- This is not mobile responsive TODO -->
            <div class="row">
              <div class="col-lg-4">
                <form title="Add to a vault">
                  <select name="add-to-vault-select" id="">
                    <option>Add to Vault</option>
                  </select>
                </form>
              </div>
              <div class="col-lg-4">
                <i class="mdi mdi-delete fs-1 mx-4"></i>
              </div>
              <div class="col-lg-4">
                <div>
                  <img
                    :src="activeKeep?.creator.picture"
                    alt=""
                    class="img-fluid thumbnail-img"
                  />
                  {{ activeKeep?.creator.name }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
  </Modal>
</template>


<script>
import { computed } from '@vue/reactivity'
import { AppState } from '../AppState'
export default {
  setup() {
    return {
      activeKeep: computed(() => AppState.activeKeep)
    }
  }
}
</script>


<style lang="scss" scoped>
.position {
  position: absolute;
  bottom: 1em;
  right: 1em;
}

.thumbnail-img {
  height: 3em;
  border-radius: 50%;
  margin-left: 3em;
  cursor: pointer;
}
</style>
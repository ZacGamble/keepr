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
            <div class="row">
              <div class="d-flex responsive-div">
                <button class="btn btn-danger">Remove From Vault</button>
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

.responsive-div {
  background-color: var(primary);
  align-items: center;
  transform: translateY(2em);
}

@media only screen and (max-width: 1200px) {
  .responsive-div {
    transform: translateX(0em);
    align-items: center;
  }
}

@media only screen and (max-width: 992px) {
  .responsive-div {
    transform: translateX(-12em);
    align-items: center;
  }
}
@media only screen and (max-width: 768px) {
  .responsive-div {
    transform: translateX(0em);
    align-items: center;
  }
}

.thumbnail-img {
  height: 3em;
  border-radius: 50%;
  margin-left: 3em;
  cursor: pointer;
}
</style>
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

            <div class="d-flex responsive-div">
              <form title="Add to a vault" @submit.prevent="addKeepToVault()">
                <select
                  name="add-to-vault-select"
                  v-model="vaultSelect"
                  required
                >
                  <option v-for="mv in myVaults" :key="mv" :value="mv">
                    {{ mv?.name.substring(0, 15) }}
                  </option>
                </select>
                <button class="btn btn-secondary mt-2 ms-2">
                  Add To Vault
                </button>
              </form>
              <i
                v-if="activeKeep?.creatorId == account.id"
                class="mdi mdi-delete fs-1 ms-4 action"
                title="delete keep"
                @click="deleteKeep()"
              ></i>
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
    </template>
  </Modal>
</template>


<script>
import { computed, ref } from '@vue/reactivity'
import { AppState } from '../AppState'
import { accountService } from '../services/AccountService'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { onMounted } from '@vue/runtime-core'
import { AuthService } from '../services/AuthService'
import { vaultKeepsService } from '../services/VaultKeepsService'
import { keepsService } from '../services/KeepsService'
import { Modal } from 'bootstrap'
import { vaultsService } from '../services/VaultsService'
export default {
  setup() {
    const vaultSelect = ref({})
    return {
      vaultSelect,
      myVaults: computed(() => AppState.myVaults),
      activeKeep: computed(() => AppState.activeKeep),
      account: computed(() => AppState.account),

      async addKeepToVault() {
        try {
          await vaultKeepsService.addKeepToVault(vaultSelect.value)
          Pop.toast("Keep added to your vault!", 'success')
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async deleteKeep() {
        try {
          await keepsService.deleteKeep()
          Pop.toast("Keep disposed of. Successfully.")
          Modal.getOrCreateInstance(document.getElementById("keep-modal")).hide()
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
.position {
  position: absolute;
  bottom: 1em;
  right: 1em;
}

.responsive-div {
  background-color: var(primary);
  align-items: center;
  transform: translateY(0em);
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
    margin-top: 2em;
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
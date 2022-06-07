<template>
  <Modal id="keep-modal">
    <template #modal-body-slot>
      <div class="container-fluid">
        <div class="row">
          <div class="col-lg-6">
            <img
              :src="activeKeep?.img"
              alt="keep image"
              class="img-fluid rounded keep-img"
            />
          </div>
          <!-- Begin right side of the modal -->
          <div class="col-lg-6">
            <div class="row">
              <div class="d-flex justify-content-evenly align-items-center">
                <div class="d-flex">
                  <i class="mdi mdi-eye mx-4 fs-2" />
                  <span class="fs-2">{{ activeKeep?.views }}</span>
                </div>
                <div class="d-flex">
                  <i class="mdi mdi-safe mx-4 fs-2" />
                  <span class="fs-2">{{ activeKeep?.kept }}</span>
                </div>
                <button
                  type="button"
                  class="btn-close position"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                ></button>
              </div>
            </div>
            <!-- The title and description -->
            <div class="row mt-5 mb-5">
              <div class="p-2 border-bottom border-dark">
                <h1 class="text-center">{{ activeKeep?.name }}</h1>
                <p class="mb-4">{{ activeKeep?.description }}</p>
                <div class="d-flex justify-content-end">
                  <i
                    v-if="activeKeep?.creatorId == account.id"
                    class="mdi mdi-delete fs-1 ms-4 action trash"
                    title="delete keep"
                    @click="deleteKeep()"
                  ></i>
                </div>
              </div>
            </div>

            <!-- The troublemaker -->
            <div
              class="d-flex responsive-div onethirdheight mb-1"
              style="transform: translateY(-2em)"
            >
              <form @submit.prevent="addKeepToVault()">
                <label class="p-2" for="add-to-vault-select"
                  ><small>Add To Vault:</small></label
                >
                <select
                  name="add-to-vault-select"
                  v-model="vaultSelect"
                  class="action"
                  title="Add to a vault"
                  required
                >
                  <option
                    v-for="mv in myVaults"
                    :key="mv"
                    :value="mv"
                    class="action"
                  >
                    {{ mv?.name.substring(0, 15) }}
                  </option>
                </select>
                <button
                  class="
                    mdi mdi-check-outline
                    m-2
                    border
                    p-1
                    bg-primary
                    selectable
                  "
                  title="Submit"
                  type="submit"
                ></button>
              </form>

              <div class="mt-1 theProfile">
                <img
                  :src="activeKeep?.creator.picture"
                  alt=""
                  class="img-fluid thumbnail-img"
                  :title="`Go to the profile of ${activeKeep?.creator.name}`"
                  @click="goToProfile()"
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
import { useRouter } from 'vue-router'
export default {
  setup() {
    const vaultSelect = ref({})
    const router = useRouter();
    return {
      vaultSelect,
      router,
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
          if (await Pop.confirm()) {
            await keepsService.deleteKeep()
            Pop.toast("Keep destroyed.")
            Modal.getOrCreateInstance(document.getElementById("keep-modal")).hide()
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },

      async goToProfile() {
        try {
          const profileId = AppState.activeKeep.creatorId
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
.onethirdheight {
  height: 33%;
}
.position {
  position: relative;
}
.trash {
  text-shadow: 1px 1px 1px black;
}
.trash:hover {
  transform: scale(1.03);
  cursor: pointer;
}

.keep-img {
  max-height: 100%;
}

@media only screen and (max-width: 991.8px) {
  .position {
    position: relative;
    margin-left: 20px;
    margin-right: 20px;
  }
}
@media only screen and (max-width: 991.8px) {
  .theProfile {
    position: relative;
  }
}

.responsive-div {
  background-color: var(primary);
  align-items: center;
  transform: translateY(0em);
}

@media only screen and (max-width: 1200px) {
  .responsive-div {
    transform: translateX(0em);
    flex-direction: column;
  }
}

@media only screen and (max-width: 992px) {
  .responsive-div {
    transform: translateX(-0em);
    transform: translateY(-1em);
    align-items: center;
    justify-content: center;
    flex-direction: column;
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
  margin-left: 2em;
  cursor: pointer;
}
</style>
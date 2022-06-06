<template>
  <Modal id="new-vault-modal">
    <template #modal-body-slot>
      <div class="d-flex-flex-column p-3 my-3">
        <h1 class="border-bottom">New Vault</h1>
        <form class="fs-5" @submit.prevent="createNewVault(formData)">
          <div class="d-flex flex-column">
            <input
              type="text"
              placeholder="Title..."
              class="mt-4 p-2 rounded"
              v-model="formData.name"
              required
            />
            <input
              type="text"
              placeholder="Description..."
              class="mt-4 p-2 rounded"
              v-model="formData.description"
              required
            />
            <input
              type="text"
              placeholder="Image URL..."
              class="mt-4 p-2 rounded"
              v-model="formData.img"
              required
            />
            <div class="d-flex align-items-center">
              <input
                type="checkbox"
                name="private-button"
                class="me-4 my-5"
                v-model="formData.isPrivate"
              />
              <label for="private-button">Private?</label><br /><small
                class="ms-4"
                >Private vaults can only be seen by you</small
              >
            </div>
            <button class="btn btn-primary w-25 mt-2">Create</button>
          </div>
        </form>
      </div>
    </template>
  </Modal>
</template>


<script>
import { ref } from '@vue/reactivity'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { vaultsService } from '../services/VaultsService'
import { Modal } from 'bootstrap'
export default {
  setup() {
    const formData = ref({})
    return {
      formData,


      async createNewVault() {
        try {
          await vaultsService.create(formData.value)
          Modal.getOrCreateInstance(document.getElementById('new-vault-modal')).hide()
          Pop.toast("Creation success!", 'success')
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
</style>
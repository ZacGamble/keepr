<template>
  <Modal id="new-keep-modal">
    <template #modal-body-slot>
      <div class="d-flex-flex-column">
        <h1>New Keep</h1>
        <form class="fs-5" @submit.prevent="createNewKeep()">
          <div class="d-flex flex-column">
            <input
              type="text"
              placeholder="Title..."
              required
              minlength="3"
              maxlength="20"
              class="my-3 p-2 rounded"
              v-model="formData.name"
            />
            <input
              type="url"
              placeholder="URL..."
              class="my-3 p-2 rounded"
              required
              v-model="formData.img"
            />
            <textarea
              placeholder="Keep description"
              maxlength="120"
              class="p-2 my-2 rounded"
              v-model="formData.description"
            ></textarea>
          </div>
          <button class="btn btn-primary">Create</button>
        </form>
      </div>
    </template>
  </Modal>
</template>


<script>
import { ref } from '@vue/reactivity'
import { keepsService } from '../services/KeepsService'
import { logger } from '../utils/Logger'
import Pop from '../utils/Pop'
import { Modal } from 'bootstrap'
export default {
  setup() {
    const formData = ref({})

    return {
      formData,

      async createNewKeep() {
        try {

          await keepsService.create(formData.value)
          Modal.getOrCreateInstance(document.getElementById('new-keep-modal')).hide()
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
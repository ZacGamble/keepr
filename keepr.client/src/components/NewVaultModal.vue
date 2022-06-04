<template>
  <Modal id="new-vault-modal">
    <template #modal-body-slot>
      <div class="d-flex-flex-column p-3 my-3">
        <h1 class="border-bottom">New Vault</h1>
        <form class="fs-5">
          <div class="d-flex flex-column">
            <input type="text" placeholder="Title..." />
            <div class="d-flex align-items-center">
              <input type="checkbox" name="private-button" class="me-4 my-4" />
              <label for="private-button">Private?</label><br /><small
                class="ms-4"
                >Private vaults can only be seen by you</small
              >
            </div>
            <button type="submit">Create</button>
          </div>
        </form>
      </div>
    </template>
  </Modal>
</template>


<script>
export default {
  setup() {
    return {}
  }
}
</script>


<style lang="scss" scoped>
</style>
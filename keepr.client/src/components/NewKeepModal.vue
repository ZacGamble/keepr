<template>
  <Modal id="new-keep-modal">
    <template #modal-body-slot>
      <div class="d-flex-flex-column">
        <h1>New Keep</h1>
        <form class="fs-5">
          <div class="d-flex flex-column">
            <input
              type="text"
              placeholder="Title..."
              required
              minlength="3"
              maxlength="20"
              class="my-3 p-2 rounded"
            />
            <input
              type="url"
              placeholder="URL..."
              class="my-3 p-2 rounded"
              required
            />
            <textarea
              placeholder="Keep description"
              maxlength="120"
              class="p-2 my-2 rounded"
            ></textarea>
          </div>
          <button class="btn btn-primary">Create</button>
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
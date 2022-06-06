<template>
  <!-- Modal -->
  <div class="modal fade" id="modelId" tabindex="-1">
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="d-flex justify-content-end"></div>
        <div class="modal-body">
          <slot name="modal-body-slot"> </slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  setup() {
    return {}
  }
}
</script>

<style lang="scss" scoped>
.position {
  margin-right: 1em;
  margin-top: 1em;
}
</style>
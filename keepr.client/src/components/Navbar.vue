<template>
  <nav
    class="
      navbar navbar-expand-lg
      bg-secondary
      px-3
      justify-content-between
      d-flex
    "
  >
    <router-link class="navbar-brand d-flex" :to="{ name: 'Home' }">
      <div class="d-flex flex-column align-items-center">
        <h2>
          <em class="p-1 fw-bold fs-1 shade">Keepr</em>
        </h2>
      </div>
    </router-link>
    <button
      class="navbar-toggler"
      type="button"
      data-bs-toggle="collapse"
      data-bs-target="#navbarText"
      aria-controls="navbarText"
      aria-expanded="false"
      aria-label="Toggle navigation"
    >
      <span class="navbar-toggler-icon" />
    </button>
    <div class="collapse navbar-collapse" id="navbarText">
      <!-- LOGIN COMPONENT HERE -->
    </div>
    <Login />
  </nav>
</template>

<script>
export default {
  setup() {
    return {};
  },
};
</script>

<style scoped>
.shade {
  text-shadow: 4px 4px 3px black;
}
.skew {
  transform: skew(-0.06turn, 18deg);
}
</style>

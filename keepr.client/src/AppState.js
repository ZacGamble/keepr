import { reactive } from 'vue'

// NOTE AppState is a reactive object to contain app level data
export const AppState = reactive({
  user: {},
  account: {},
  keeps: [],
  myVaults: [],
  userKeeps: [],
  vaultKeeps: [],
  userVaults: [],
  activeKeep: null,
  activeProfile: null,
  profiles: []
})

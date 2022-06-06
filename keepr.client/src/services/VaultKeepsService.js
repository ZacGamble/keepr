import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class VaultKeepsService{

    async addKeepToVault(vault){
        const vaultKeep = {}
        vaultKeep.vaultId = vault.id
        vaultKeep.keepId = AppState.activeKeep.id
        const res = await api.post('api/vaultkeeps', vaultKeep)
        const modifiedKeep = AppState.activeKeep
        
        modifiedKeep.kept++
        const incrementRes = await api.put("api/keeps/"+modifiedKeep.id, modifiedKeep)
        // logger.log("The incremented keep", res.data)
    }
    async getKeepsByVaultId(vaultId){
        const res = await api.get(`api/vaults/${vaultId}/keeps`)
        AppState.vaultKeeps = res.data
        logger.log("the keeps in this vault > ", res.data)
    }

    async removeFromVault(keepId){
        await api.delete("api/vaultkeeps/" + keepId)
        //FIXME this is the where the lack of vaultKeepId's makes things fall apart
        
    }
}

export const vaultKeepsService = new VaultKeepsService();
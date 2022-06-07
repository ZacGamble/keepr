import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class VaultsService {

    async create(formData){
        formData.creatorId = AppState.account.id
        const res = await api.post("api/vaults", formData)
        AppState.userVaults.unshift(res.data);
        AppState.myVaults.unshift(res.data);
    }

    async getById(vaultId){
        const res = await api.get("api/vaults/" + vaultId)
        AppState.activeVault = res.data
    }

    async getUserVaults(profileId){
        const res = await api.get("api/profiles/"+ profileId + "/vaults")
        AppState.userVaults = res.data;
        AppState.userVaults.reverse();
    }

    async deleteVault(vaultId){
        await api.delete("api/vaults/" + vaultId)
        const index = AppState.myVaults.find(v => v.id == vaultId)
        AppState.myVaults.splice(index, 1)
    }
}

export const vaultsService = new VaultsService()
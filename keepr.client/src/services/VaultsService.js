import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class VaultsService {

    async create(formData){
        formData.creatorId = AppState.account.id
        const res = await api.post("api/vaults", formData)
        AppState.userVaults.unshift(res.data);
        logger.log('created vault > vaultsService.js >', res.data)
    }

    async getUserVaults(profileId){
        const res = await api.get("api/profiles/"+ profileId + "/vaults")
        AppState.userVaults = res.data;
        AppState.userVaults.reverse();
        logger.log("Get user vaults > ", AppState.userVaults)
    }

    async deleteVault(vaultId){
        await api.delete("api/vaults/" + vaultId)
        const index = AppState.userVaults.find(v => v.id == vaultId)
        AppState.userVaults.splice(index, 1)
    }
}

export const vaultsService = new VaultsService()
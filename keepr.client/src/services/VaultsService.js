import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class VaultsService {

    async create(formData){
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
}

export const vaultsService = new VaultsService()
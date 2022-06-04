import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class VaultsService {

    async getUserVaults(profileId){
        const res = await api.get("api/profiles/"+ profileId + "/vaults")
        AppState.userVaults = res.data;
        logger.log("Get user vaults > ", AppState.userVaults)
    }
}

export const vaultsService = new VaultsService()
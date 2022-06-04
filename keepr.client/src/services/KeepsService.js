import { AppState } from"../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class KeepsService {
    async getAll(){
        const res = await api.get("api/keeps")
        AppState.keeps = res.data;
        logger.log("Get All / set active Keeps > ", AppState.keeps)
    }
    async getUserKeeps(profileId){
        const res = await api.get("api/profiles/"+ profileId + "/keeps")
        AppState.userKeeps = res.data;
        logger.log("Get user Keeps > ", AppState.userKeeps)
    }
}

export const keepsService = new KeepsService();
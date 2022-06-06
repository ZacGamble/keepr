import { AppState } from"../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class KeepsService {
    async create(formData){
        const res = await api.post("api/keeps", formData)
        AppState.userKeeps.unshift(res.data)
        logger.log(res.data)
    }
    async getAll(){
        const res = await api.get("api/keeps")
        AppState.keeps = res.data;
        AppState.keeps.reverse()
        logger.log("Get All / set active Keeps > ", AppState.keeps)
    }
    async getUserKeeps(profileId){
        const res = await api.get("api/profiles/"+ profileId + "/keeps")
        AppState.keeps = res.data;
        AppState.keeps.reverse()
        logger.log("Get user Keeps > ", AppState.keeps)
    }

    async deleteKeep(){
        const id = AppState.activeKeep.id
        await api.delete("api/keeps/" + id)
        const index = AppState.keeps.find(k => k.id == id)
        AppState.keeps.splice(index, 1)
    }
}

export const keepsService = new KeepsService();
import { AppState } from "../AppState";
import { logger } from "../utils/Logger";
import { api } from "./AxiosService";

class ProfilesService {

    async getUserProfile(profileId)
    {
        const res = await api.get("api/profiles/" + profileId)
        AppState.activeProfile = res.data
        logger.log("The active profile > profiles Services >", AppState.activeProfile)
    }
}

export const profilesService = new ProfilesService();
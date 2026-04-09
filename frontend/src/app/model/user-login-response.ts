import { UserResponse } from "./user-response";

export interface UserLoginResponse {
    user: UserResponse,
    token: string,
}

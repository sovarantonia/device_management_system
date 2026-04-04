import { DeviceSummary } from "./device-summary";

export interface UserResponse {
    id: string,
    name: string,
    role: string,
    email: string,
    location: string,
    devices: DeviceSummary[],
}

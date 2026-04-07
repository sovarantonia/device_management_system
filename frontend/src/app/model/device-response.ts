import { UserResponse } from "./user-response";

export interface DeviceResponse {
    id: string;
    name: string;
    manufacturer: string;
    deviceType: string;
    os: string;
    osVersion: string;
    processor: string;
    ramAmount: number;
    description: string;
    user: UserResponse | null;
}

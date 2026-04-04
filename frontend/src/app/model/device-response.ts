import { UserSummary } from "./user-summary";

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
    user: UserSummary | null;
}

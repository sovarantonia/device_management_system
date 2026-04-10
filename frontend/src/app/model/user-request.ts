export interface UserRequest {
    name: string,
    role?: string | null,
    email: string,
    location?: string | null,
    password: string,
}

import { HttpClient } from "@angular/common/http";
import { inject, Injectable, PLATFORM_ID } from "@angular/core";
import { UserRequest } from "../../model/user-request";
import { Observable, tap } from "rxjs";
import { RegisterResponse } from "../../model/register-response";
import { UserLoginRequest } from "../../model/user-login-request";
import { UserLoginResponse } from "../../model/user-login-response";
import { isPlatformBrowser } from "@angular/common";
import { UserResponse } from "../../model/user-response";

@Injectable({ providedIn: "root" })
export class AuthService {
    private readonly baseUrl = 'https://localhost:7064';
    private readonly http: HttpClient = inject(HttpClient);
    private platformId = inject(PLATFORM_ID);

    register(userRequest: UserRequest): Observable<RegisterResponse> {
        return this.http.post<RegisterResponse>(`${this.baseUrl}/register`, userRequest);
    }

    login(userLoginRequest: UserLoginRequest): Observable<UserLoginResponse> {
        return this.http.post<UserLoginResponse>(`${this.baseUrl}/login`, userLoginRequest).pipe(
            tap(res => { this.setToken(res.token); this.setCurrentUser(res.user) })
        );
    }

    setToken(token: string): void {
        if (isPlatformBrowser(this.platformId)) {
            sessionStorage.setItem('token', token);
        }
    }

    setCurrentUser(user: UserResponse) {
        if (isPlatformBrowser(this.platformId)) {
            sessionStorage.setItem('user', JSON.stringify(user));
        }
    }

    logout(): void {
        if (isPlatformBrowser(this.platformId)) {
            sessionStorage.removeItem('token');
            sessionStorage.removeItem('user');
        }
    }

    getToken(): string | null {
        if (isPlatformBrowser(this.platformId)) {
            return sessionStorage.getItem('token');
        }

        return null;
    }

    getCurrentUser() {
        if (isPlatformBrowser(this.platformId)) {
            const user = sessionStorage.getItem('user');
            return user ? JSON.parse(user) : null;
        }

        return null;
    }

    getCurrentUserId(): string | null {
        const token = this.getToken();
        if (!token) {
            return null;
        }
        const payload = JSON.parse(atob(token.split('.')[1]));

        return payload.sub ?? null;
    }

}

import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { UserRequest } from "../../model/user-request";
import { Observable } from "rxjs";
import { RegisterResponse } from "../../model/register-response";

@Injectable({providedIn: "root"})
export class AuthService {
    private readonly baseUrl = 'https://localhost:7064';
    private readonly http: HttpClient = inject(HttpClient);

    register(userRequest: UserRequest): Observable<RegisterResponse> {
        return this.http.post<RegisterResponse>(`${this.baseUrl}/register`, userRequest);
    }

}

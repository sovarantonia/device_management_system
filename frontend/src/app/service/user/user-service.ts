import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserRequest } from '../../model/user-request';
import { Observable } from 'rxjs';
import { UserResponse } from '../../model/user-response';

@Injectable({ providedIn: 'root' })
export class UserService {
    private readonly baseUrl = 'https://localhost:7064/user';
    private readonly http: HttpClient = inject(HttpClient);


    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    update(id: string, userRequest: UserRequest): Observable<UserResponse> {
        return this.http.put<UserResponse>(`${this.baseUrl}/${id}`, userRequest);
    }

    getById(id: string): Observable<UserResponse> {
        return this.http.get<UserResponse>(`${this.baseUrl}/${id}`);
    }

    getAll(): Observable<UserResponse[]> {
        return this.http.get<UserResponse[]>(this.baseUrl);
    }

    findByEmail(email: string): Observable<UserResponse> {
        return this.http.get<UserResponse>(`${this.baseUrl}/search/${email}`);
    }
}

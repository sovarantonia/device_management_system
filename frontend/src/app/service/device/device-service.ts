import { HttpClient, HttpParams } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { DeviceResponse } from "../../model/device-response";
import { Observable } from 'rxjs';
import { DeviceRequest } from "../../model/device-request";
@Injectable({ providedIn: 'root' })
export class DeviceService {
    private readonly baseUrl = 'https://localhost:7064/device';
    private readonly http: HttpClient = inject(HttpClient);

    getAll(): Observable<DeviceResponse[]> {
        return this.http.get<DeviceResponse[]>(this.baseUrl);
    }

    getById(id: string): Observable<DeviceResponse> {
        return this.http.get<DeviceResponse>(`${this.baseUrl}/${id}`)
    }

    save(deviceRequest: DeviceRequest): Observable<DeviceResponse> {
        return this.http.post<DeviceResponse>(this.baseUrl, deviceRequest);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    updateDetails(id: string, deviceRequest: DeviceRequest): Observable<DeviceResponse> {
        return this.http.put<DeviceResponse>(`${this.baseUrl}/${id}`, deviceRequest);
    }

    getByUser(userId: string): Observable<DeviceResponse[]> {
        return this.http.get<DeviceResponse[]>(`${this.baseUrl}/user/${userId}`);
    }

    assignDevice(deviceId: string): Observable<DeviceResponse> {
        return this.http.put<DeviceResponse>(`${this.baseUrl}/${deviceId}/assign`, null);
    }

    unassignDevice(deviceId: string): Observable<DeviceResponse> {
        return this.http.put<DeviceResponse>(`${this.baseUrl}/${deviceId}/unassign`, null);
    }

    generateDescription(deviceRequest: DeviceRequest): Observable<string> {
        return this.http.post(`${this.baseUrl}/generate-description`, deviceRequest, {
            responseType: 'text'
        });
    }

    search(query: string): Observable<DeviceResponse[]> {
        const params = new HttpParams().set('query', query);
        return this.http.get<DeviceResponse[]>(`${this.baseUrl}/search`, { params });
    }
}

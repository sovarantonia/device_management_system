import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "./auth-service";

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authService = inject(AuthService);
        const token = authService.getToken();

        const isPublicEndpoint =
            req.url.includes('/login') ||
            req.url.includes('/register');

        if (!token || isPublicEndpoint) {
            return next.handle(req);
        } else {
            const cloned = req.clone({
                headers: req.headers.set("Authorization", `Bearer ${token}` )
            });

            return next.handle(cloned);
        }
    }
}

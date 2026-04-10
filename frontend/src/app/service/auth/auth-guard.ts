import { inject, Injectable, PLATFORM_ID } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "./auth-service";
import { isPlatformBrowser } from "@angular/common";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    platformId = inject(PLATFORM_ID);
    constructor(private authService: AuthService, private router: Router) { }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {
        if (!isPlatformBrowser(this.platformId)) {
            return true;
        }

        if (this.authService.getToken()) {
            return true;
        }

        return this.router.createUrlTree(['/login'], {
            queryParams: { returnUrl: state.url }
        });
    }
}

import { Routes } from '@angular/router';
import { ViewDevice } from './view-device/view-device';
import { CreateDevice } from './create-device/create-device';
import { DevicePage } from './device-page/device-page';
import { UserRegister } from './user-register/user-register';
import { UserLogin } from './user-login/user-login';
import { AuthGuard } from './service/auth/auth-guard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: UserLogin },
    { path: 'devices', component: DevicePage, canActivate: [AuthGuard] },
    { path: 'device/new', component: CreateDevice, canActivate: [AuthGuard] },
    { path: 'device/:id', component: ViewDevice, canActivate: [AuthGuard] },
    { path: 'register', component: UserRegister },   
];

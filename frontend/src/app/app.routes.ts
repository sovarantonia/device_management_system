import { Routes } from '@angular/router';
import { ViewDevice } from './view-device/view-device';
import { CreateDevice } from './create-device/create-device';
import { DevicePage } from './device-page/device-page';

export const routes: Routes = [
    { path: 'devices', component: DevicePage },
    { path: 'device/new', component: CreateDevice },
    { path: 'device/:id', component: ViewDevice },
];

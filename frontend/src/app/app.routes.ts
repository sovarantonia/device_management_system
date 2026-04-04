import { Routes } from '@angular/router';
import { DeviceRender } from './device-render/device-render';
import { ViewDevice } from './view-device/view-device';

export const routes: Routes = [
    {path: 'devices', component: DeviceRender},
    {path: 'devices/:id', component: ViewDevice}
];

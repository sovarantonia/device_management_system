import { Routes } from '@angular/router';
import { DeviceRender } from './device-render/device-render';
import { ViewDevice } from './view-device/view-device';
import { CreateDeviceForm } from './create-device-form/create-device-form';
import { DevicePage } from './device-page/device-page';

export const routes: Routes = [
    { path: 'devices', component: DevicePage },
    { path: 'device/new', component: CreateDeviceForm },
    { path: 'device/:id', component: ViewDevice },
    
];

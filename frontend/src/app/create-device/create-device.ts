import { Component, EventEmitter, Output } from '@angular/core';
import { DeviceRequest } from '../model/device-request';
import { DeviceService } from '../service/device/device-service';
import { Router } from '@angular/router';
import { DeviceForm } from "../device-form/device-form";
import { SnackbarService } from '../service/snackbar/snackbar-service';

@Component({
  selector: 'app-create-device',
  imports: [DeviceForm],
  templateUrl: './create-device.html',
  styleUrl: './create-device.css',
})
export class CreateDevice {
  formTitle = "Add new device";

  constructor(private deviceService: DeviceService, private router: Router, private snackbarService: SnackbarService) {}

  save(deviceRequest: DeviceRequest) {
    this.deviceService.save(deviceRequest).subscribe({
      next: () => {
        this.snackbarService.open('Device saved', 'success');
        this.router.navigate(['/devices']);
      },
      error: (err) => {
        this.snackbarService.open(err.message || 'Could not save device', 'error');
      }
   })
  }
}


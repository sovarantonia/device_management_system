import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceRender } from "../device-render/device-render";
import { DeviceService } from '../service/device/device-service';
import { DeviceResponse } from '../model/device-response';
import { SnackbarService } from '../service/snackbar/snackbar-service';
import { AuthService } from '../service/auth/auth-service';

@Component({
  selector: 'app-device-page',
  imports: [DeviceRender],
  templateUrl: './device-page.html',
  styleUrl: './device-page.css',
})
export class DevicePage implements OnInit {
  devices: DeviceResponse[] = [];
  constructor(private deviceService: DeviceService, private router: Router, private snackbarService: SnackbarService, private authService: AuthService) { }

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
      },
      error: (err) => {
        this.snackbarService.open('Error loading devices', 'error');
      }
    })
  }

  viewDevice(deviceId: string) {
    this.router.navigate(['device', deviceId]);
  }

  onAddDeviceClick() {
    this.router.navigate(['device/new'])
  }

  onDeleteDeviceClick(deviceId: string) {
    const confirmed = window.confirm('Are you sure you want to delete this device?');

    if (!confirmed) {
      return;
    }

    this.deviceService.delete(deviceId).subscribe({
      next: () => {
        this.snackbarService.open('Device was deleted', 'success')
        this.loadDevices()
      },
      error: (err) => this.snackbarService.open('Could not delete device', 'error')
    });

    this.loadDevices();
  }

  onLogoutBtnClicked() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

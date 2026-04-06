import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceRender } from "../device-render/device-render";
import { DeviceService } from '../service/device/device-service';
import { DeviceResponse } from '../model/device-response';

@Component({
  selector: 'app-device-page',
  imports: [DeviceRender],
  templateUrl: './device-page.html',
  styleUrl: './device-page.css',
})
export class DevicePage implements OnInit {
  devices: DeviceResponse[] = [];
  constructor(private deviceService: DeviceService, private router: Router) { }

  ngOnInit(): void {
    this.loadDevices();
  }

  loadDevices(): void {
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
      },
      error: (err) => {
        console.error('Error loading devices', err);
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
      next: () => this.loadDevices(),
      error: (err) => console.error('Error deleting device', err)
    });

    this.loadDevices();
  }
}

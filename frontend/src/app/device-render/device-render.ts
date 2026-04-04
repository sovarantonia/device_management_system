import { Component, OnInit } from '@angular/core';
import { DeviceService } from '../service/device/device-service';
import { DeviceResponse } from '../model/device-response';

@Component({
  selector: 'app-device-render',
  imports: [],
  templateUrl: './device-render.html',
  styleUrl: './device-render.css',
})
export class DeviceRender implements OnInit {
  devices: DeviceResponse[] = [];

  constructor(private deviceService: DeviceService) { }

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
}

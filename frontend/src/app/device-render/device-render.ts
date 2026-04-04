import { Component, inject, OnInit } from '@angular/core';
import { DeviceService } from '../service/device/device-service';
import { DeviceResponse } from '../model/device-response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-device-render',
  imports: [],
  templateUrl: './device-render.html',
  styleUrl: './device-render.css',
})
export class DeviceRender implements OnInit {
  devices: DeviceResponse[] = [];
  private router = inject(Router);

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

    viewDevice(id: string) {
      this.router.navigate(['/devices', id]);
    }
}

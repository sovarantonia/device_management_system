import { Component, OnInit } from '@angular/core';
import { DeviceResponse } from '../model/device-response';
import { DeviceService } from '../service/device/device-service';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DeviceForm } from "../device-form/device-form";
import { DeviceRequest } from '../model/device-request';

@Component({
  selector: 'app-view-device',
  imports: [ReactiveFormsModule, DeviceForm],
  templateUrl: './view-device.html',
  styleUrl: './view-device.css',
})
export class ViewDevice implements OnInit {
  device: DeviceResponse | null = null;
  errorMessage = '';
  id: string | null = null;

  constructor(private deviceService: DeviceService, private route: ActivatedRoute, private router: Router) {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.getDevice();
  }

  getDevice() {
    if (this.id) {
      this.deviceService.getById(this.id).subscribe({
        next: (data) => {
          this.device = data;
        }
      })
    }
  }

  updateDetails(deviceRequest: DeviceRequest) {
    if (this.id) {
      this.deviceService.updateDetails(this.id, deviceRequest).subscribe({
        next: () => {
        alert('Device updated');
        this.router.navigate(['/devices']);
      },
        error: (err) => {
        alert(err.error?.message || 'Could not update device.')
        this.errorMessage = err.error?.message || 'Could not update device.';
      }
      })
    }

  }

  
}

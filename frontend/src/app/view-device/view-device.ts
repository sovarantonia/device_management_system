import { Component, OnInit } from '@angular/core';
import { DeviceResponse } from '../model/device-response';
import { DeviceService } from '../service/device/device-service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-view-device',
  imports: [],
  templateUrl: './view-device.html',
  styleUrl: './view-device.css',
})
export class ViewDevice implements OnInit {
  device: DeviceResponse | null = null;

  constructor(private deviceService: DeviceService, private route: ActivatedRoute, private router: Router) {

  }

  ngOnInit(): void {
    this.getDevice();
  }

  getDevice() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.deviceService.getById(id).subscribe({
        next: (data) => {
          this.device = data;
        }
      })
    }
  }

  close() {
    this.router.navigate(['/devices']);
  }
}

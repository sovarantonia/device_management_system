import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceRender } from "../device-render/device-render";

@Component({
  selector: 'app-device-page',
  imports: [DeviceRender],
  templateUrl: './device-page.html',
  styleUrl: './device-page.css',
})
export class DevicePage implements OnInit {
  constructor(private router: Router) { }

  ngOnInit(): void {
    
  }
  
  onAddDeviceClick() {
    this.router.navigate(['device/new'])
  }
}

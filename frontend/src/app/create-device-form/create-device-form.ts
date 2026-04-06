import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DeviceRequest } from '../model/device-request';
import { DeviceService } from '../service/device/device-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-device-form',
  imports: [ReactiveFormsModule],
  templateUrl: './create-device-form.html',
  styleUrl: './create-device-form.css',
})
export class CreateDeviceForm {
  createDeviceForm!: FormGroup;
  errorMessage = '';

  constructor(private formBuilder: FormBuilder, private deviceService: DeviceService, private router: Router) {
    this.createDeviceForm = this.formBuilder.group({
      deviceName: ['', Validators.required],
      deviceManufacturer: ['', Validators.required],
      deviceType: ['', Validators.required],
      deviceOS: ['', Validators.required],
      deviceOSVersion: ['', Validators.required],
      deviceProcessor: ['', Validators.required],
      deviceRamAmount: [0, [Validators.required, Validators.min(0)]],
      deviceDescription: ['', Validators.required],
    })
  }

  onSubmit(event: Event) {
    event.preventDefault();
    if (this.createDeviceForm.valid) {
      const formValue = this.createDeviceForm.value;
      const device: DeviceRequest = {
        name: formValue.deviceName,
        manufacturer: formValue.deviceManufacturer,
        deviceType: formValue.deviceType,
        os: formValue.deviceOS,
        osVersion: formValue.deviceOSVersion,
        processor: formValue.deviceProcessor,
        ramAmount: formValue.deviceRamAmount,
        description: formValue.deviceDescription
      };
      this.deviceService.save(device).subscribe({
        next: () => {
          window.alert("Device created");
          this.router.navigate(['/devices']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Could not create device.';
        }
      });
    }
  }
}


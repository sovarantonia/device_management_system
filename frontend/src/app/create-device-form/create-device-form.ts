import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { DeviceRequest } from '../model/device-request';

@Component({
  selector: 'app-create-device-form',
  imports: [ReactiveFormsModule],
  templateUrl: './create-device-form.html',
  styleUrl: './create-device-form.css',
})
export class CreateDeviceForm {
  createDeviceForm!: FormGroup;

  constructor(private formBuilder: FormBuilder) {
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
      const device = this.createDeviceForm.value as DeviceRequest;
    }
  }
}

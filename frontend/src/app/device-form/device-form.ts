import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DeviceRequest } from '../model/device-request';
import { DeviceResponse } from '../model/device-response';
import { Router } from '@angular/router';
import { DeviceService } from '../service/device/device-service';
import { SnackbarService } from '../service/snackbar/snackbar-service';

@Component({
  selector: 'app-device-form',
  imports: [ReactiveFormsModule],
  templateUrl: './device-form.html',
  styleUrl: './device-form.css',
})
export class DeviceForm implements OnInit, OnChanges {
  @Input() initialData: DeviceResponse | null = null;
  @Input() formTitle: string | null = null;
  @Output() formSubmitted = new EventEmitter<DeviceRequest>();

  deviceForm!: FormGroup;

  isGenerating = false;

  constructor(private formBuilder: FormBuilder, private router: Router, private deviceService: DeviceService, private snackbarService: SnackbarService) { }

  ngOnInit(): void {
    this.deviceForm = this.formBuilder.group({
      deviceName: ['', Validators.required],
      deviceManufacturer: ['', Validators.required],
      deviceType: ['', Validators.required],
      deviceOS: ['', Validators.required],
      deviceOSVersion: ['', Validators.required],
      deviceProcessor: ['', Validators.required],
      deviceRamAmount: [0, [Validators.required, Validators.min(0)]],
      deviceDescription: ['', [Validators.required, Validators.maxLength(255)]],
    })
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['initialData'] && this.initialData && this.deviceForm) {
      this.deviceForm.patchValue({
        deviceName: this.initialData.name,
        deviceManufacturer: this.initialData.manufacturer,
        deviceType: this.initialData.deviceType,
        deviceOS: this.initialData.os,
        deviceOSVersion: this.initialData.osVersion,
        deviceProcessor: this.initialData.processor,
        deviceRamAmount: this.initialData.ramAmount,
        deviceDescription: this.initialData.description
      });
    }
  }

  onSubmit() {
    if (this.deviceForm.invalid) {
      this.deviceForm.markAllAsTouched();
      return;
    }

    const formValue = this.deviceForm.value;
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

    this.formSubmitted.emit(device);
  }

  close() {
    this.router.navigate(['/devices']);
  }

  onGenerateDescClick() {
    this.isGenerating = true;
    const formValue = this.deviceForm.value;
    const device: DeviceRequest = {
      name: formValue.deviceName,
      manufacturer: formValue.deviceManufacturer,
      deviceType: formValue.deviceType,
      os: formValue.deviceOS,
      osVersion: formValue.deviceOSVersion,
      processor: formValue.deviceProcessor,
      ramAmount: formValue.deviceRamAmount,
    };
    this.deviceService.generateDescription(device).subscribe({
      next: (data) => {
        this.deviceForm.patchValue({
          deviceDescription: data
        })
        this.isGenerating = false;
      },
      error: () => {
        this.snackbarService.open('Could not generate a description', 'error');
        this.isGenerating = false;
      }
    })
  }

}

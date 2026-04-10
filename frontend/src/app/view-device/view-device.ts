import { Component, OnInit } from '@angular/core';
import { DeviceResponse } from '../model/device-response';
import { DeviceService } from '../service/device/device-service';
import { ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DeviceForm } from "../device-form/device-form";
import { DeviceRequest } from '../model/device-request';
import { SnackbarService } from '../service/snackbar/snackbar-service';
import { AuthService } from '../service/auth/auth-service';

@Component({
  selector: 'app-view-device',
  imports: [ReactiveFormsModule, DeviceForm],
  templateUrl: './view-device.html',
  styleUrl: './view-device.css',
})
export class ViewDevice implements OnInit {
  device: DeviceResponse | null = null;
  id: string | null = null;
  formTitle = "Edit device";
  currentUserId: string | null = null;

  constructor(private deviceService: DeviceService, private route: ActivatedRoute, private snackbarService: SnackbarService, private authService: AuthService) {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.currentUserId = this.authService.getCurrentUserId();
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

  get isAssigned(): boolean {
    return !!this.device?.user;
  }

  get isAssignedToCurrentUser(): boolean {
    return !!this.device?.user && this.device.user.id === this.currentUserId;
  }

  get canAssign(): boolean {
    return !this.isAssigned;
  }

  get canUnassign(): boolean {
    return this.isAssignedToCurrentUser;
  }

  updateDetails(deviceRequest: DeviceRequest) {
    if (this.id) {
      this.deviceService.updateDetails(this.id, deviceRequest).subscribe({
        next: () => {
          this.snackbarService.open('Device updated', 'success');
        },
        error: (err) => {
          this.snackbarService.open(err.errors?.message || 'Could not update device.', 'error')
        }
      })
    }
  }

  assignDevice() {
    if (this.id) {
      this.deviceService.assignDevice(this.id).subscribe({
        next: () => {
          this.snackbarService.open('Device assigned successfully', 'success');
          this.getDevice();
        },
        error: (err) => {
          this.snackbarService.open(err.error?.message || 'Could not assign device', 'error');
        }
      })
    }
  }

  unassignDevice() {
    if (this.id) {
      this.deviceService.unassignDevice(this.id).subscribe({
        next: () => {
          this.snackbarService.open('Device unassigned successfully', 'success');
          this.getDevice();
        },
        error: (err) => {
          this.snackbarService.open(err.error?.message || 'Could not unassign device', 'error');
        }
      })
    }
  }


}

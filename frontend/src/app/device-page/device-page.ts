import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeviceRender } from "../device-render/device-render";
import { DeviceService } from '../service/device/device-service';
import { DeviceResponse } from '../model/device-response';
import { SnackbarService } from '../service/snackbar/snackbar-service';
import { AuthService } from '../service/auth/auth-service';
import { UserResponse } from '../model/user-response';
import { FormsModule } from '@angular/forms';
import { catchError, debounceTime, distinctUntilChanged, of, Subject, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-device-page',
  imports: [DeviceRender, FormsModule],
  templateUrl: './device-page.html',
  styleUrl: './device-page.css',
})
export class DevicePage implements OnInit, OnDestroy {
  devices: DeviceResponse[] = [];
  currentUser!: UserResponse;
  searchQuery = '';

  private searchSubject = new Subject<string>();
  private searchSubscription?: Subscription;

  constructor(private deviceService: DeviceService, private router: Router, private snackbarService: SnackbarService, private authService: AuthService) { }

  ngOnInit(): void {
    this.currentUser = this.authService.getCurrentUser();
    this.loadDevices();

    this.searchSubscription = this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((query) => {
        const trimmedQuery = query.trim();

        if (!trimmedQuery) {
          return this.deviceService.getAll();
        }

        return this.deviceService.search(trimmedQuery).pipe(
          catchError(() => {
            this.snackbarService.open('Could not load devices', 'error');
            return of([]);
          })
        );
      })
    ).subscribe((devices) => {
      this.devices = devices;
    });
  }

  ngOnDestroy(): void {
    this.searchSubscription?.unsubscribe();
  }

  loadDevices(): void {
    this.deviceService.getAll().subscribe({
      next: (data) => {
        this.devices = data;
      },
      error: (err) => {
        this.snackbarService.open('Error loading devices', 'error');
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
      next: () => {
        this.snackbarService.open('Device deleted', 'success')
        this.loadDevices();
      },
      error: (err) => this.snackbarService.open('Could not delete device', 'error')
    });

  }

  onSearchInput() {
    this.searchSubject.next(this.searchQuery);
  }

  onLogoutBtnClicked() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

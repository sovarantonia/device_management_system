import { Component, inject, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DeviceResponse } from '../model/device-response';

@Component({
  selector: 'app-device-render',
  imports: [],
  templateUrl: './device-render.html',
  styleUrl: './device-render.css',
})
export class DeviceRender {
  @Input() devices: DeviceResponse[] = [];
  @Output() viewDeviceClicked = new EventEmitter<string>();
  @Output() deleteDeviceClicked = new EventEmitter<string>();

  onViewClick(id: string) {
    this.viewDeviceClicked.emit(id);
  }

  onDeleteClick(id: string) {
    this.deleteDeviceClicked.emit(id);
  }
}

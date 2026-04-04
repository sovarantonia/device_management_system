import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DeviceRender } from "./device-render/device-render";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, DeviceRender],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('frontend');
}

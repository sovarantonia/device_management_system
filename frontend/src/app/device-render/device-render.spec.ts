import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceRender } from './device-render';

describe('DeviceRender', () => {
  let component: DeviceRender;
  let fixture: ComponentFixture<DeviceRender>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeviceRender]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeviceRender);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

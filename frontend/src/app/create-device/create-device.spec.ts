import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateDeviceForm } from './create-device-form';

describe('CreateDeviceForm', () => {
  let component: CreateDeviceForm;
  let fixture: ComponentFixture<CreateDeviceForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateDeviceForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateDeviceForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

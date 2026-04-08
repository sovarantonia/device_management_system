import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-register',
  imports: [ReactiveFormsModule],
  templateUrl: './user-register.html',
  styleUrl: './user-register.css',
})
export class UserRegister implements OnInit {
  registerForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    })
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
  }

}

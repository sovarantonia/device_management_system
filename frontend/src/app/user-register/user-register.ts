import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserRequest } from '../model/user-request';
import { AuthService } from '../service/auth/auth-service';
import { confirmPasswordValidator } from '../validator/password-validator';
import { EmailExistsValidator } from '../validator/email-exists-validator';
import { SnackbarService } from '../service/snackbar/snackbar-service';

@Component({
  selector: 'app-user-register',
  imports: [ReactiveFormsModule],
  templateUrl: './user-register.html',
  styleUrl: './user-register.css',
})
export class UserRegister implements OnInit {
  registerForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private emailExistsValidator: EmailExistsValidator, private snackbarService: SnackbarService) { }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email], [this.emailExistsValidator.checkEmailExists()]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    },
      {
        validators: confirmPasswordValidator
      }
    )
  }

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }

    const formValue = this.registerForm.value;
    const userRequest: UserRequest = {
      name: formValue.userName,
      email: formValue.email,
      password: formValue.password,
    }

    this.authService.register(userRequest).subscribe({
      next: (data) => {
        this.snackbarService.open(data.message, "success");
      },
      error: () => {
        this.snackbarService.open("Something went wrong", "error");
      }
    })

  }

}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../service/auth/auth-service';
import { UserLoginRequest } from '../model/user-login-request';
import { SnackbarService } from '../service/snackbar/snackbar-service';

@Component({
  selector: 'app-user-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './user-login.html',
  styleUrl: './user-login.css',
})
export class UserLogin implements OnInit{
  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private authService: AuthService, private snackbarService: SnackbarService) { }
  
  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.email, Validators.required]],
      password: ['', Validators.required]
    })
  }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    const formValue = this.loginForm.value;
    const userLoginRequest: UserLoginRequest = {
      email: formValue.email,
      password: formValue.password,
    }

    this.authService.login(userLoginRequest).subscribe({
      next: () => {
        this.router.navigate(['/devices']);
      },
      error: () => {
        this.snackbarService.open('Could not log in. Check your credentials', 'error');
      }
    })
  }
}

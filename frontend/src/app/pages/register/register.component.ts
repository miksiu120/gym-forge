import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { Router, RouterModule, RouterLinkActive } from '@angular/router';
import { CreateUserDto } from '../../interfaces/account.interfaces';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { AccountService } from '../../services/account-service/account-service.service';

@Component({
  selector: 'page-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [RouterModule, RouterLinkActive, ReactiveFormsModule, NgIf],
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage = '';
  successMessage = '';
  submitting = false;
  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private router: Router,
  ) {
    this.registerForm = this.formBuilder.group({
      nickname: ['', [Validators.required]],
      email: ['', [Validators.required]],
      name: ['', []],
      surname: ['', []],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
    });
  }

  register() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    this.errorMessage = '';
    this.successMessage = '';
    this.submitting = true;
    const registerData: CreateUserDto = this.registerForm.value;
    this.accountService.registerAccount(registerData).subscribe({
      next: () => {
        this.submitting = false;
        this.successMessage = 'Account created. Redirecting to sign in...';
        setTimeout(() => this.router.navigate(['/login']), 500);
      },
      error: (error) => {
        this.submitting = false;
        this.errorMessage = error.error?.message ?? 'We could not create your account. Please review the form.';
      },
    });
  }
}

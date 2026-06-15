import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { LoginUserDto } from '../../interfaces/account.interfaces';
import { AccountService } from '../../services/account-service/account-service.service';
import { TokenService } from '../../services/token-service/token-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [ReactiveFormsModule, NgIf],
  providers: [FormBuilder],
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage = '';
  submitting = false;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private tokenService: TokenService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      nickname: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.errorMessage = '';
    this.submitting = true;
    const loginData: LoginUserDto = this.loginForm.value;
    this.accountService.loginAccount(loginData).subscribe({
      next: (response) => {
        this.tokenService.saveTokensToLocalStorage(response);
        this.router.navigate(['/dashboard/training-panel']);
      },
      error: (error) => {
        this.submitting = false;
        this.errorMessage = error.error?.message ?? 'We could not sign you in. Check your nickname and password.';
      },
    });
  }

  hasError(controlName: string, errorType: string): boolean {
    const control = this.loginForm.get(controlName);
    return !!control && control.hasError(errorType) && control.touched;
  }
}

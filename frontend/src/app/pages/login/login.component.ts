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
      return;
    }

    const loginData: LoginUserDto = this.loginForm.value;
    console.log('Logging in with data:', loginData);
    this.accountService.loginAccount(loginData).subscribe({
      next: (response) => {
        console.log('Login successful!', response);
        this.tokenService.saveTokensToLocalStorage(response);
        this.router.navigate(['/dashboard/training-panel']);
      },
      error: (err) => {
        console.error('Login failed!', err);
      },
    });
  }

  hasError(controlName: string, errorType: string): boolean {
    const control = this.loginForm.get(controlName);
    return !!control && control.hasError(errorType) && control.touched;
  }
}

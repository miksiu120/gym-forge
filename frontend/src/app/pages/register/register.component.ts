import { Component } from '@angular/core';
import { RouterModule, RouterLinkActive } from '@angular/router';
import { CreateUserDto } from '../../interfaces/account.interfaces';
import {
  Form,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { AccountService } from '../../services/account-service/account-service.service';
import { Route } from '@angular/router';

@Component({
  selector: 'page-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [RouterModule, RouterLinkActive, ReactiveFormsModule],
  providers: [FormBuilder],
})
export class RegisterComponent {
  registerForm: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService
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
    const registerData: CreateUserDto = this.registerForm.value;
    console.log('registration:', registerData);
    this.accountService.registerAccount(registerData).subscribe({
      next: (response) => {
        console.log('User created, ', response);
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}

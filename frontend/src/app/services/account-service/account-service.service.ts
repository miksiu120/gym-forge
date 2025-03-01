import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import {
  CreateUserDto,
  LoginUserDto,
  LoginResponse,
} from '../../interfaces/account.interfaces';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient) {}

  getApiUrl(): string {
    return environment.apiUrl;
  }
  getApiUrlWithEndpoint(endpoint: string): string {
    return `${environment.apiUrl}/${endpoint}`;
  }

  registerAccount(registerData: CreateUserDto): Observable<void> {
    return this.http.post<void>(
      this.getApiUrlWithEndpoint('accounts/register'),
      registerData
    );
  }

  loginAccount(loginData: LoginUserDto): Observable<LoginResponse> {
    return this.http.put<LoginResponse>(
      this.getApiUrlWithEndpoint('accounts/login'),
      loginData
    );
  }

 
}

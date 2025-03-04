import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import {
  CreateUserDto,
  LoginUserDto,
  LoginResponse,
  AccountDetailsDto,
} from '../../interfaces/account.interfaces';

import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  getAccountDetails(): Observable<AccountDetailsDto> {
    const token = localStorage.getItem('session_token');
    console.log('Token autoryzacji:', token);
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<AccountDetailsDto>(
      this.getApiUrlWithEndpoint('accounts/details'),
      { headers }
    );
  }
}

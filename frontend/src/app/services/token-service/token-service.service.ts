import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { LoginResponse } from '../../interfaces/account.interfaces';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private helper = new JwtHelperService();
  private authStatusSubject = new BehaviorSubject<boolean>(
    this.isAuthenticated()
  );

  public authStatus$ = this.authStatusSubject.asObservable();

  isAuthenticated(): boolean {
    const token = localStorage.getItem('session_token');
    return !!token && !this.helper.isTokenExpired(token);
  }

  getTokenData(): any {
    const token = localStorage.getItem('session_token');
    return token ? this.helper.decodeToken(token) : null;
  }

  saveTokensToLocalStorage(loginResponse: LoginResponse) {
    localStorage.setItem('session_token', loginResponse.token);
    localStorage.setItem('refresh_token', loginResponse.refreshToken);
    localStorage.setItem('name', loginResponse.nickname);
    console.log('Ustawiono tokeny poprawnie');
    this.authStatusSubject.next(true);
  }

  clearTokensFromLocalStorage() {
    localStorage.removeItem('sesson_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('name');
    this.authStatusSubject.next(false);
  }
}

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

    console.log('token:', !!token && !this.helper.isTokenExpired(token));
    if (token == null || this.helper.isTokenExpired(token)) {
      return false;
    } else {
      return true;
    }
  }

  getTokenData(): any {
    const token = localStorage.getItem('session_token');
    return token ? this.helper.decodeToken(token) : null;
  }

  saveTokensToLocalStorage(loginResponse: LoginResponse) {
    localStorage.setItem('session_token', loginResponse.token);
    localStorage.setItem('refresh_token', loginResponse.refreshToken);
    localStorage.setItem('nickname', loginResponse.nickname);
    console.log('Ustawiono tokeny poprawnie');
    this.authStatusSubject.next(true);
  }

  clearTokensFromLocalStorage() {
    localStorage.removeItem('session_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('nickname');
    this.authStatusSubject.next(false);
  }
}

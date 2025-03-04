import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { TokenService } from './token-service.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private auth: TokenService, private router: Router) {}

  canActivate(): boolean {
    if (this.auth.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/login'], {
      queryParams: { returnUrl: this.router.url },
    });
    return false;
  }
}

@Injectable({
  providedIn: 'root',
})
export class GuestGuard implements CanActivate {
  constructor(private auth: TokenService, private router: Router) {}

  canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/dashboard/training-panel'], {
      queryParams: { returnUrl: this.router.url },
    });
    return false;
  }
}

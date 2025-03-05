import { Component } from '@angular/core';
import { BurgerMenuComponent } from './burger-menu/burger-menu.component';
import { RouterLinkActive, RouterModule } from '@angular/router';
import { TokenService } from '../../../services/token-service/token-service.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'component-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
  imports: [BurgerMenuComponent, RouterLinkActive, RouterModule],
})
export class NavbarComponent {
  isLoggedIn = false;
  isBurgerActive = false;
  userName = '';
  private authSubscription!: Subscription;

  constructor(private tokenService: TokenService) {}

  ngOnInit() {
    this.isLoggedIn = this.tokenService.isAuthenticated();
    this.userName = localStorage.getItem('nickname') || 'user';

    this.authSubscription = this.tokenService.authStatus$.subscribe(
      (status) => {
        this.isLoggedIn = status;
        this.userName = localStorage.getItem('nickname') || 'user';
      }
    );
  }

  toggleBurger() {
    this.isBurgerActive = !this.isBurgerActive;
  }

  logutUser() {
    this.tokenService.clearTokensFromLocalStorage();
  }
}

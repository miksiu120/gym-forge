import { Component } from '@angular/core';
import { BurgerMenuComponent } from './burger-menu/burger-menu.component';
import { RouterLinkActive, RouterModule } from '@angular/router';

@Component({
  selector: 'component-navbar',
  imports: [BurgerMenuComponent, RouterModule, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isBurgerActive = false;
  toggleBurger() {
    this.isBurgerActive = !this.isBurgerActive;
  }
}

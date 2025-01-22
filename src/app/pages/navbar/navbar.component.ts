import { Component } from '@angular/core';
import { BurgerMenuComponent } from './burger-menu/burger-menu.component';

@Component({
  selector: 'component-navbar',
  imports: [BurgerMenuComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isBurgerActive = false;
  toggleBurger() {
    this.isBurgerActive = !this.isBurgerActive;
  }
}

import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BurgerMenuComponent } from '../navbar/burger-menu/burger-menu.component';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, BurgerMenuComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'work-planner';
}

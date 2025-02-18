import { Component, Output, EventEmitter, Input } from '@angular/core';
import { NgClass } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'burger-menu',
  imports: [NgClass, RouterLinkActive, RouterLink],
  templateUrl: './burger-menu.component.html',
  styleUrl: './burger-menu.component.scss',
})
export class BurgerMenuComponent {
  @Output() toggleBurger = new EventEmitter<void>();

  @Input() isBurgerActive = false;
  constructor() {}

  areCalculationsVisible: boolean = false;

  toggle() {
    this.toggleBurger.emit();
  }

  toggleCalculations() {
    this.areCalculationsVisible = !this.areCalculationsVisible;
  }
}

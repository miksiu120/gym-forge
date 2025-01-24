import { Component, Output, EventEmitter, Input } from '@angular/core';
import { NgClass } from '@angular/common';

@Component({
  selector: 'burger-menu',
  imports: [NgClass],
  templateUrl: './burger-menu.component.html',
  styleUrl: './burger-menu.component.scss',
})
export class BurgerMenuComponent {
  @Output() toggleBurger = new EventEmitter<void>();

  @Input() isBurgerActive = false;
  constructor() {}

  toggle() {
    this.toggleBurger.emit();
  }
}

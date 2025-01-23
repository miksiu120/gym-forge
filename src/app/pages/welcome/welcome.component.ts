import { Component } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { InstructionComponent } from './instruction/instruction.component';

@Component({
  selector: 'page-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  imports: [HeaderComponent, InstructionComponent],
})
export class WelcomeComponent {
  // Component logic goes here
}

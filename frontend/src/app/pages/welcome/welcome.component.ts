import { Component } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { InstructionComponent } from './instruction/instruction.component';
import { CalculatorsComponent } from '../calculators/calculators.component';
import { FooterComponent } from "../app/footer/footer.component";
import { CalculatorRecommendComponent } from "./calculator-recommend/calculator-recommend.component";
@Component({
  selector: 'page-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  imports: [HeaderComponent, InstructionComponent, CalculatorsComponent, FooterComponent, CalculatorRecommendComponent],
})
export class WelcomeComponent {
  // Component logic goes here
}

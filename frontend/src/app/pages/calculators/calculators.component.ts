import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'page-calculators',
  templateUrl: './calculators.component.html',
  styleUrls: ['./calculators.component.scss'],
  standalone: true,
  imports: [RouterLink],
})
export class CalculatorsComponent {
}

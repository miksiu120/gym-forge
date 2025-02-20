import { Component, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'page-wilks',
  imports: [FormsModule],
  templateUrl: './wilks.component.html',
  styleUrl: './wilks.component.scss',
})
export class WilksComponent {
  weight: number = 0;
  bodyweight: number = 0;
  gender: string = '';
  wilksScore: number = 0;
  calculateWilks() {
    const coefficients =
      this.gender === 'male'
        ? [
            -216.0475144, 16.2606339, -0.002388645, -0.00113732, 7.01863e-6,
            -1.291e-8,
          ]
        : [
            594.31747775582, -27.23842536447, 0.82112226871, -0.00930733913,
            4.731582e-5, -9.054e-8,
          ];

    let denominator =
      coefficients[0] +
      coefficients[1] * this.bodyweight +
      coefficients[2] * Math.pow(this.bodyweight, 2) +
      coefficients[3] * Math.pow(this.bodyweight, 3) +
      coefficients[4] * Math.pow(this.bodyweight, 4) +
      coefficients[5] * Math.pow(this.bodyweight, 5);

    if (denominator > 0) {
      this.wilksScore = (this.weight * 500) / denominator;
    } else {
      this.wilksScore = 0;
    }

    this.wilksScore = Math.round(this.wilksScore * 1000) / 1000;
  }
}

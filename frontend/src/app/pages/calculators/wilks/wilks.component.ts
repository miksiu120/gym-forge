import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

type WeightUnit = 'kg' | 'lb';

@Component({
  selector: 'page-wilks',
  imports: [FormsModule, RouterLink],
  templateUrl: './wilks.component.html',
  styleUrl: './wilks.component.scss',
})
export class WilksComponent {
  weight = 0;
  bodyweight = 0;
  gender: 'male' | 'female' | '' = '';
  weightUnit: WeightUnit = 'kg';
  bodyweightUnit: WeightUnit = 'kg';
  wilksScore = 0;
  hasResult = false;
  errorMessage = '';

  calculateWilks(): void {
    if (this.weight <= 0 || this.bodyweight <= 0 || !this.gender) {
      this.wilksScore = 0;
      this.hasResult = false;
      this.errorMessage = 'Enter valid lifting and body weights, then select a gender.';
      return;
    }

    this.errorMessage = '';
    const liftedWeightKg = this.toKilograms(this.weight, this.weightUnit);
    const bodyweightKg = this.toKilograms(this.bodyweight, this.bodyweightUnit);
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
      coefficients[1] * bodyweightKg +
      coefficients[2] * Math.pow(bodyweightKg, 2) +
      coefficients[3] * Math.pow(bodyweightKg, 3) +
      coefficients[4] * Math.pow(bodyweightKg, 4) +
      coefficients[5] * Math.pow(bodyweightKg, 5);

    if (denominator > 0) {
      this.wilksScore = (liftedWeightKg * 500) / denominator;
      this.hasResult = true;
    } else {
      this.wilksScore = 0;
      this.hasResult = false;
    }

    this.wilksScore = Math.round(this.wilksScore * 1000) / 1000;
  }

  private toKilograms(weight: number, unit: WeightUnit): number {
    return unit === 'lb' ? weight * 0.45359237 : weight;
  }
}

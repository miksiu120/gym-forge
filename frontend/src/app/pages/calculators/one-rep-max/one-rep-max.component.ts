import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface OneRepMaxRow {
  reps: number;
  weight: number;
  percentValue: number;
}

@Component({
  selector: 'page-one-rep-max',
  imports: [FormsModule],
  templateUrl: './one-rep-max.component.html',
  styleUrl: './one-rep-max.component.scss',
})
export class OneRepMaxComponent {
  weight = 0;
  reps = 0;
  unit: 'kg' | 'lb' = 'kg';

  table: OneRepMaxRow[] = [];
  errorMessage = '';

  createTable = () => {
    if (!this.weight || !this.reps || this.weight <= 0 || this.reps <= 0 || this.reps > 30) {
      this.table = [];
      this.errorMessage = 'Enter a weight above 0 and between 1 and 30 repetitions.';
      return;
    }
    this.errorMessage = '';
    const table: OneRepMaxRow[] = [];
    for (let i = 0; i < 10; i++) {
      const percentValue = 100 - 5 * i;
      table.push({
        reps: i + 1,
        weight: this.calculatePercentageWeight(percentValue),
        percentValue,
      });
    }
    this.table = table;
  };

  calculateOneRepMax(): number {
    return Math.round(this.weight / (1.0278 - 0.0278 * this.reps));
  }

  calculatePercentageWeight(percent: number): number {
    return Math.round(this.calculateOneRepMax() * percent / 100);
  }
}

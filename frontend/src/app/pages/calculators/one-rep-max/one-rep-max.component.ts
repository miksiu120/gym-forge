import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'page-one-rep-max',
  imports: [FormsModule],
  templateUrl: './one-rep-max.component.html',
  styleUrl: './one-rep-max.component.scss',
})
export class OneRepMaxComponent {
  weight: number = 0;
  reps: number = 0;

  table: any[] = [];
  errorMessage = '';

  createTable = () => {
    if (!this.weight || !this.reps || this.weight <= 0 || this.reps <= 0 || this.reps > 30) {
      this.table = [];
      this.errorMessage = 'Enter a weight above 0 and between 1 and 30 repetitions.';
      return;
    }
    this.errorMessage = '';
    let table = [];
    for (let i = 0; i < 10; i++) {
      table.push({
        reps: i + 1,
        weight: this.calculateNRepMax(i + 1),
        percentValue: 100 - 5 * i,
      });
    }
    this.table = table;
  };

  calculateOneRepMax(): number {
    return Math.round(this.weight / (1.0278 - 0.0278 * this.reps));
  }

  calculateNRepMax(n: number): number {
    const oneRepMax = this.calculateOneRepMax();
    return Math.round(oneRepMax * (1.0278 - 0.0278 * n));
  }
}

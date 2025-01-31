import { Component, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'page-one-rep-max',
  imports: [FormsModule, CommonModule],
  templateUrl: './one-rep-max.component.html',
  styleUrl: './one-rep-max.component.scss',
})
export class OneRepMaxComponent {
  weight: number = 0;
  reps: number = 0;

  table: any[] = [];

  createTable = () => {
    console.log(this.weight);
    console.log(this.reps);
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

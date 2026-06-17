import { Component, Input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Exercise, TrainingUnit } from '../../../interfaces/traning.interfaces';

@Component({
  selector: 'component-create-training-unit',
  imports: [ReactiveFormsModule],
  templateUrl: './create-training-unit.component.html',
  styleUrl: './create-training-unit.component.scss',
})
export class CreateTrainingUnitComponent {
  @Input() exercises: Exercise[] = [];
  @Input() units: TrainingUnit[] = [];

  readonly unitForm;
  selected = new Set<number>();
  selectionError = false;

  constructor(formBuilder: FormBuilder) {
    this.unitForm = formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(80)]],
    });
  }

  toggleExercise(index: number): void {
    this.selected.has(index) ? this.selected.delete(index) : this.selected.add(index);
    this.selectionError = false;
  }

  addUnit(): void {
    if (this.unitForm.invalid || this.selected.size === 0) {
      this.unitForm.markAllAsTouched();
      this.selectionError = this.selected.size === 0;
      return;
    }

    this.units.push({
      name: this.unitForm.controls.name.value!.trim(),
      exercises: [...this.selected].map((index) => ({ ...this.exercises[index] })),
    });
    this.unitForm.reset();
    this.selected.clear();
  }

  removeUnit(index: number): void {
    this.units.splice(index, 1);
  }

  unitLabel(index: number): string {
    return index < 26 ? String.fromCharCode(65 + index) : String(index + 1);
  }
}

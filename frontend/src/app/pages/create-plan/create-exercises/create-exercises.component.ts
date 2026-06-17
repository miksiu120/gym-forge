import { Component, Input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  Exercise,
  MeasurementSystem,
} from '../../../interfaces/traning.interfaces';

@Component({
  selector: 'component-create-exercises',
  imports: [ReactiveFormsModule],
  templateUrl: './create-exercises.component.html',
  styleUrl: './create-exercises.component.scss',
})
export class CreateExercisesComponent {
  @Input() exercises: Exercise[] = [];

  readonly MeasurementSystem = MeasurementSystem;
  readonly exerciseData;

  constructor(private readonly formBuilder: FormBuilder) {
    this.exerciseData = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(80)]],
      description: ['', Validators.maxLength(150)],
      sets: [3, [Validators.required, Validators.min(1), Validators.max(50)]],
      type: [MeasurementSystem.Repetitive, Validators.required],
      duration: [null as number | null, Validators.min(1)],
      repetitions: [10 as number | null, Validators.min(1)],
      tempo: ['', Validators.pattern(/^\d+-\d+-\d+-\d+$/)],
    });
  }

  addExercise(): void {
    const value = this.exerciseData.getRawValue();
    const needsRepetitions = value.type === MeasurementSystem.Repetitive;
    const measure = needsRepetitions ? value.repetitions : value.duration;

    if (this.exerciseData.invalid || !measure || measure < 1) {
      this.exerciseData.markAllAsTouched();
      return;
    }

    this.exercises.push({
      name: value.name!.trim(),
      description: value.description?.trim() || undefined,
      sets: Number(value.sets),
      type: value.type!,
      repetitions: needsRepetitions ? Number(value.repetitions) : null,
      duration: needsRepetitions ? null : Number(value.duration),
      tempo: needsRepetitions && value.tempo ? value.tempo : null,
    });

    this.exerciseData.reset({
      sets: 3,
      repetitions: 10,
      type: MeasurementSystem.Repetitive,
    });
  }

  removeExercise(index: number): void {
    this.exercises.splice(index, 1);
  }
}

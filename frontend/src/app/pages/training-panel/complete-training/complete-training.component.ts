import { DatePipe, DecimalPipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import {
  CompleteTrainingUnit,
  Exercise,
  ExerciseSetResult,
  MeasurementSystem,
  TrainingUnit,
} from '../../../interfaces/traning.interfaces';
import { TrainingPlanService } from '../../../services/training-plan-service/training-plan.service';

@Component({
  selector: 'app-complete-training',
  imports: [DatePipe, DecimalPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './complete-training.component.html',
  styleUrl: './complete-training.component.scss',
})
export class CompleteTrainingComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly trainingPlans = inject(TrainingPlanService);

  readonly measurementSystem = MeasurementSystem;
  readonly form = this.fb.group({
    notes: ['', Validators.maxLength(1000)],
    exercises: this.fb.array<FormGroup>([]),
  });

  unit?: TrainingUnit;
  loading = true;
  saving = false;
  errorMessage = '';

  get exerciseForms(): FormArray<FormGroup> {
    return this.form.controls.exercises;
  }

  ngOnInit(): void {
    const unitId = Number(this.route.snapshot.paramMap.get('unitId'));
    if (!Number.isInteger(unitId) || unitId < 1) {
      this.loading = false;
      this.errorMessage = 'This training could not be found.';
      return;
    }

    this.trainingPlans.getUnit(unitId).subscribe({
      next: (unit) => {
        this.unit = unit;
        this.form.controls.notes.setValue(unit.notes ?? '');
        unit.exercises.forEach((exercise) => this.exerciseForms.push(this.createExerciseForm(exercise)));
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'This training could not be loaded.';
        this.loading = false;
      },
    });
  }

  setsFor(exerciseIndex: number): FormArray<FormGroup> {
    return this.exerciseForms.at(exerciseIndex).controls['sets'] as FormArray<FormGroup>;
  }

  submit(): void {
    if (!this.unit?.id || this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    const payload: CompleteTrainingUnit = {
      notes: this.form.controls.notes.value?.trim() || null,
      exercises: this.unit.exercises.map((exercise, exerciseIndex) => ({
        exerciseId: exercise.id!,
        sets: this.setsFor(exerciseIndex).getRawValue() as ExerciseSetResult[],
      })),
    };

    this.saving = true;
    this.errorMessage = '';
    this.trainingPlans.completeUnit(this.unit.id, payload).subscribe({
      next: () => this.router.navigate(['/dashboard/training-panel']),
      error: (error: HttpErrorResponse) => {
        this.errorMessage = error.error?.message ?? 'The results could not be saved. Check the fields and try again.';
        this.saving = false;
      },
    });
  }

  private createExerciseForm(exercise: Exercise): FormGroup {
    const sets = this.fb.array<FormGroup>([]);
    for (let index = 0; index < exercise.sets; index++) {
      const existing = exercise.setResults?.find((result) => result.setNumber === index + 1);
      sets.push(this.fb.group({
        setNumber: [index + 1, [Validators.required, Validators.min(1)]],
        weight: [existing?.weight ?? null, [Validators.min(0), Validators.max(2000)]],
        repetitions: [
          existing?.repetitions ?? (exercise.type === MeasurementSystem.Repetitive ? exercise.repetitions : null),
          exercise.type === MeasurementSystem.Repetitive ? [Validators.required, Validators.min(0)] : [],
        ],
        duration: [
          existing?.duration ?? (exercise.type === MeasurementSystem.Timed ? exercise.duration : null),
          exercise.type === MeasurementSystem.Timed ? [Validators.required, Validators.min(0)] : [],
        ],
        rpe: [existing?.rpe ?? null, [Validators.min(0), Validators.max(10)]],
      }));
    }

    return this.fb.group({ exerciseId: [exercise.id], sets });
  }
}

import { Component } from '@angular/core';
import {
  Exercise,
  MeasurementSystem,
} from '../../../interfaces/traning.interfaces';

import {
  Form,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgIf } from '@angular/common';
import { copyFileSync } from 'fs';
import { register } from 'module';

@Component({
  selector: 'component-create-exercises',
  imports: [NgIf, ReactiveFormsModule],
  providers: [FormBuilder],
  templateUrl: './create-exercises.component.html',
  styleUrl: './create-exercises.component.scss',
})
export class CreateExercisesComponent {
  createdExercises: Exercise[] = [];
  exerciseData: FormGroup;
  MeasurementSystem = MeasurementSystem;
  constructor(private formBuilder: FormBuilder) {
    this.exerciseData = this.formBuilder.group({
      name: ['', Validators.required],
      description: [''],
      sets: ['', Validators.required],
      type: ['repetetive', Validators.required],
      duration: [, Validators.min(1)],
      repetitions: [, Validators.min(1)],
      tempo: [, Validators.pattern("\\d+-\\d+-\\d+-\\d+")]
    });
  }

  addExcercise() {
    const newExercise: Exercise = this.exerciseData.value;

    if (newExercise.type === MeasurementSystem.Repetitive) {
      newExercise.duration = null;
    } else if (newExercise.type === MeasurementSystem.Timed) {
      newExercise.repetitions = null;
    }
    console.log('New Exercise:', newExercise.type);
    this.createdExercises.push(newExercise);
    this.exerciseData.reset();
  }
}

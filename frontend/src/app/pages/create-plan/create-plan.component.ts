import { Component } from '@angular/core';
import { CreateExercisesComponent } from './create-exercises/create-exercises.component';
import { CreateTrainingUnitComponent } from "./create-training-unit/create-training-unit.component";
@Component({
  selector: 'page-create-plan',
  imports: [CreateExercisesComponent, CreateTrainingUnitComponent],
  templateUrl: './create-plan.component.html',
  styleUrl: './create-plan.component.scss',
})
export class CreatePlanComponent {}

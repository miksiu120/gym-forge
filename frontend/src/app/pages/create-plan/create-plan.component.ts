import { NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import {
  CreateTrainingPlan,
  Exercise,
  TrainingUnit,
} from '../../interfaces/training.interfaces';
import { TrainingPlanService } from '../../services/training-plan-service/training-plan.service';
import { CreateExercisesComponent } from './create-exercises/create-exercises.component';
import { CreateTrainingUnitComponent } from './create-training-unit/create-training-unit.component';
import { SetDatesComponent } from './set-dates/set-dates.component';

@Component({
  selector: 'page-create-plan',
  imports: [CreateExercisesComponent, CreateTrainingUnitComponent, SetDatesComponent, NgClass],
  templateUrl: './create-plan.component.html',
  styleUrl: './create-plan.component.scss',
})
export class CreatePlanComponent {
  actualStage = 0;
  exercises: Exercise[] = [];
  units: TrainingUnit[] = [];
  message = '';
  saving = false;

  readonly stages = ['Create exercises', 'Compose cycle days', 'Plan cycle'];

  constructor(
    private readonly trainingPlanService: TrainingPlanService,
    private readonly router: Router,
  ) {}

  stageClass(index: number): string {
    if (index < this.actualStage) return 'completed';
    return index === this.actualStage ? 'active' : 'unactive';
  }

  goNextStage(): void {
    this.message = '';
    if (this.actualStage === 0 && this.exercises.length === 0) {
      this.message = 'Add at least one exercise before continuing.';
      return;
    }
    if (this.actualStage === 1 && this.units.length === 0) {
      this.message = 'Compose at least one cycle day before continuing.';
      return;
    }
    if (this.actualStage < 2) this.actualStage++;
  }

  goPreviousStage(): void {
    this.message = '';
    if (this.actualStage > 0) this.actualStage--;
  }

  savePlan(plan: CreateTrainingPlan): void {
    this.saving = true;
    this.message = '';
    this.trainingPlanService.create(plan).subscribe({
      next: () => this.router.navigate(['/dashboard/trainings'], {
        state: { planCreated: true },
      }),
      error: (error: HttpErrorResponse) => {
        this.saving = false;
        if (error.status === 0) {
          this.message = 'The training API is unavailable. Start the backend and try again.';
          return;
        }
        if (error.status === 401) {
          this.message = 'Your session has expired. Sign in again before saving the plan.';
          return;
        }
        this.message = error.error?.message
          ?? this.firstValidationError(error.error?.errors)
          ?? 'We could not save the plan. Review the schedule and try again.';
      },
    });
  }

  private firstValidationError(errors: Record<string, string[]> | undefined): string | undefined {
    if (!errors) return undefined;
    return Object.values(errors).flat()[0];
  }
}

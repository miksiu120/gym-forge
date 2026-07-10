import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DueTrainingUnit } from '../../../interfaces/training.interfaces';
import { TrainingPlanService } from '../../../services/training-plan-service/training-plan.service';

@Component({
  selector: 'component-dashboard',
  imports: [DatePipe, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  dueTrainings: DueTrainingUnit[] = [];
  loading = true;
  loadError = '';
  readonly welcomeString = `Welcome back ${localStorage.getItem('nickname') ?? ''}`.trim();

  constructor(private readonly trainingPlans: TrainingPlanService) {}

  ngOnInit(): void {
    const through = new Date();
    through.setHours(23, 59, 59, 999);
    this.trainingPlans.getDue(through).subscribe({
      next: (trainings) => {
        this.dueTrainings = trainings;
        this.loading = false;
      },
      error: () => {
        this.loadError = 'We could not load your training list. Try again in a moment.';
        this.loading = false;
      },
    });
  }

  isOverdue(startTime?: string): boolean {
    if (!startTime) return false;
    const trainingDay = new Date(startTime);
    const today = new Date();
    trainingDay.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);
    return trainingDay < today;
  }
}

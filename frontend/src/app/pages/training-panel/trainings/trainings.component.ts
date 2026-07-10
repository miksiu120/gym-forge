import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { TrainingPlanSummary } from '../../../interfaces/training.interfaces';
import { TrainingPlanService } from '../../../services/training-plan-service/training-plan.service';

@Component({
  selector: 'app-trainings',
  imports: [DatePipe, RouterLink],
  templateUrl: './trainings.component.html',
  styleUrl: './trainings.component.scss',
})
export class TrainingsComponent {
  plans: TrainingPlanSummary[] = [];
  loading = true;
  loadFailed = false;
  created = false;

  constructor(service: TrainingPlanService, router: Router) {
    this.created = router.getCurrentNavigation()?.extras.state?.['planCreated'] === true;
    service.getMine().subscribe({
      next: (plans) => { this.plans = plans; this.loading = false; },
      error: () => { this.loadFailed = true; this.loading = false; },
    });
  }
}

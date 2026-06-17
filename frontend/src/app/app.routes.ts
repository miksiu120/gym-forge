import { Routes } from '@angular/router';

import { CalculatorsComponent } from './pages/calculators/calculators.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { WelcomeComponent } from './pages/welcome/welcome.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { WilksComponent } from './pages/calculators/wilks/wilks.component';
import { OneRepMaxComponent } from './pages/calculators/one-rep-max/one-rep-max.component';
import { StandardsComponent } from './pages/calculators/standards/standards.component';

import { TrainingPanelComponent } from './pages/training-panel/training-panel.component';
import { StatisticsComponent } from './pages/training-panel/statistics/statistics.component';
import { DashboardComponent } from './pages/training-panel/dashboard/dashboard.component';
import { TrainingsComponent } from './pages/training-panel/trainings/trainings.component';
import { ProfileComponent } from './pages/profile/profile.component';
import {
  AuthGuard,
  GuestGuard,
} from './services/token-service/auth-guard.service';
import { ProfileEditComponent } from './pages/profile-edit/profile-edit.component';
import { CreatePlanComponent } from './pages/create-plan/create-plan.component';
import { CompleteTrainingComponent } from './pages/training-panel/complete-training/complete-training.component';

export const routes: Routes = [
  { path: '', component: WelcomeComponent, canActivate: [GuestGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'calculators', component: CalculatorsComponent },
  { path: 'calculators/wilks', component: WilksComponent },
  { path: 'calculators/one-rep-max', component: OneRepMaxComponent },
  { path: 'calculators/standards', component: StandardsComponent },

  {
    path: 'dashboard',
    component: TrainingPanelComponent,
    children: [
      { path: '', redirectTo: 'training-panel', pathMatch: 'full' },
      { path: 'training-panel', component: DashboardComponent },
      { path: 'statistics', component: StatisticsComponent },
      { path: 'trainings', component: TrainingsComponent },
      { path: 'complete-training/:unitId', component: CompleteTrainingComponent },
    ],
    canActivate: [AuthGuard],
  },

  {
    path: 'create-plan',
    component: CreatePlanComponent,
    canActivate: [AuthGuard],
  },

  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
  {
    path: 'profile-edit',
    component: ProfileEditComponent,
    canActivate: [AuthGuard],
  },
  { path: '**', component: NotFoundComponent },
];

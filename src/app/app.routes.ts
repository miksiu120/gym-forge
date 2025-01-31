import { Routes } from '@angular/router';

import { CalculatorsComponent } from './pages/calculators/calculators.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { WelcomeComponent } from './pages/welcome/welcome.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { WilksComponent } from './pages/calculators/wilks/wilks.component';
import { OneRepMaxComponent } from './pages/calculators/one-rep-max/one-rep-max.component';
import { StandardsComponent } from './pages/calculators/standards/standards.component';

export const routes: Routes = [
  { path: '', component: WelcomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'calculators', component: CalculatorsComponent },
  { path: 'calculators/wilks', component: WilksComponent },
  { path: 'calculators/one-rep-max', component: OneRepMaxComponent },
  { path: 'calculators/standards', component: StandardsComponent },
  { path: '**', component: NotFoundComponent },
];

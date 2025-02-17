import { Component } from '@angular/core';
import { RouterLinkActive, RouterModule } from '@angular/router';
import { SideNavComponent } from './side-nav/side-nav.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { StandardsComponent } from '../calculators/standards/standards.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'component-training-panel',
  imports: [ SideNavComponent,RouterOutlet],
  templateUrl: './training-panel.component.html',
  styleUrl: './training-panel.component.scss',
})
export class TrainingPanelComponent {}

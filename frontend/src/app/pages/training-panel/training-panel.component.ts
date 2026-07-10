import { Component } from '@angular/core';
import { SideNavComponent } from './side-nav/side-nav.component';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'component-training-panel',
  imports: [SideNavComponent, RouterOutlet],
  templateUrl: './training-panel.component.html',
  styleUrl: './training-panel.component.scss',
})
export class TrainingPanelComponent {}

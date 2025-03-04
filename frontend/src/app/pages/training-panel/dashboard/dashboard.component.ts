import { Component } from '@angular/core';

@Component({
  selector: 'component-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent {
  welcomeString: string = '';
  nearestTrainings: number = 0;
  ngOnInit() {
    this.welcomeString = `Welcome back ${localStorage.getItem('nickname')}`;
  }
}

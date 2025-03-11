import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'component-dashboard',
  imports: [RouterLink, RouterLinkActive],
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

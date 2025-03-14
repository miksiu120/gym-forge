import { Component } from '@angular/core';
import { CreateExercisesComponent } from './create-exercises/create-exercises.component';
import { CreateTrainingUnitComponent } from './create-training-unit/create-training-unit.component';
import { SetDatesComponent } from './set-dates/set-dates.component';
import { NgClass } from '@angular/common';

enum Status {
  Unactive = 'unactive',
  Active = 'active',
  Completed = 'completed',
}

@Component({
  selector: 'page-create-plan',
  imports: [
    CreateExercisesComponent,
    CreateTrainingUnitComponent,
    SetDatesComponent,
    NgClass,
  ],
  templateUrl: './create-plan.component.html',
  styleUrl: './create-plan.component.scss',
})
export class CreatePlanComponent {
  actualStage: number = 0;
  statusStages: Status[] = [Status.Active, Status.Unactive, Status.Unactive];

  actualIcon = `
      <i class="fa-solid fa-location-pin"></i>
  `;

  completedIcon = `
      <i class="fa-solid fa-check"></i>
  `;
  noIcon = ``;

  stageIcons: any[] = [this.actualIcon, this.noIcon, this.noIcon];
  GoNextStage() {
    if (this.actualStage >= 2) return;

    this.actualStage++;
    this.statusStages[this.actualStage - 1] = Status.Completed;
    this.stageIcons[this.actualStage - 1] = this.completedIcon;

    this.statusStages[this.actualStage] = Status.Active;
    this.stageIcons[this.actualStage] = this.actualIcon;
  }

  GoPreviousStage() {
    if (this.actualStage <= 0) return;

    this.actualStage--;
    this.statusStages[this.actualStage] = Status.Active;
    this.stageIcons[this.actualStage] = this.actualIcon;

    this.statusStages[this.actualStage + 1] = Status.Unactive;
    this.stageIcons[this.actualStage + 1] = this.noIcon;
  }

  OnNextButton() {}
}

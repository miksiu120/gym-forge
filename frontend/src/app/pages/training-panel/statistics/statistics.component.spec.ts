import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticsComponent } from './statistics.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TrainingStatistics } from '../../../interfaces/training.interfaces';

describe('StatisticsComponent', () => {
  let component: StatisticsComponent;
  let fixture: ComponentFixture<StatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatisticsComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should select and cycle through exercise series', () => {
    component.statistics = statisticsWithExercises;
    spyOn<any>(component, 'renderWeightChart').and.resolveTo();

    component.selectExercise(1);
    expect(component.selectedExercise?.exerciseName).toBe('Bench press');

    component.moveExercise(1);
    expect(component.selectedExercise?.exerciseName).toBe('Back squat');

    component.moveExercise(-1);
    expect(component.selectedExercise?.exerciseName).toBe('Bench press');
  });

  it('should expose latest and maximum recorded weights', () => {
    const series = statisticsWithExercises.weightProgress[0];

    expect(component.latestWeight(series)).toBe(105);
    expect(component.maximumWeight(series)).toBe(110);
  });
});

const statisticsWithExercises: TrainingStatistics = {
  completedSessions: 3,
  completedSets: 9,
  totalVolume: 2500,
  recentSessions: [],
  weightProgress: [
    {
      exerciseName: 'Back squat',
      points: [
        { completedAt: '2026-07-20T10:00:00Z', weight: 110 },
        { completedAt: '2026-07-25T10:00:00Z', weight: 105 },
      ],
    },
    {
      exerciseName: 'Bench press',
      points: [{ completedAt: '2026-07-24T10:00:00Z', weight: 80 }],
    },
  ],
};

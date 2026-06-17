import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { TrainingPanelComponent } from './training-panel.component';

describe('TrainingPanelComponent', () => {
  let component: TrainingPanelComponent;
  let fixture: ComponentFixture<TrainingPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrainingPanelComponent],
      providers: [provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(TrainingPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

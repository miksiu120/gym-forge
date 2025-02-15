import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingPanelComponent } from './training-panel.component';

describe('TrainingPanelComponent', () => {
  let component: TrainingPanelComponent;
  let fixture: ComponentFixture<TrainingPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrainingPanelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrainingPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

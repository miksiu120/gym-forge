import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetDatesComponent } from './set-dates.component';
import { ExerciseType } from '../../../interfaces/training.interfaces';

describe('SetDatesComponent', () => {
  let component: SetDatesComponent;
  let fixture: ComponentFixture<SetDatesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SetDatesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SetDatesComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('units', [{
      name: 'Training A',
      exercises: [{
        name: 'Squat',
        sets: 3,
        repetitions: 5,
        type: ExerciseType.Repetitive,
      }],
    }]);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should expand a one-week cycle into scheduled sessions', () => {
    const emittedPlans: unknown[] = [];
    component.savePlan.subscribe((plan) => emittedPlans.push(plan));
    component.scheduleForm.patchValue({
      name: 'Strength block',
      from: '2026-07-13',
      to: '2026-07-26',
      cycleWeeks: 1,
    });
    component.slots.at(0).patchValue({ cycleWeek: 1, dayOfWeek: 1, time: '18:00' });

    component.submit();

    expect(emittedPlans.length).toBe(1);
    expect((emittedPlans[0] as { trainingUnits: unknown[] }).trainingUnits.length).toBe(2);
  });

  it('should explain why an incomplete plan cannot be saved', () => {
    component.scheduleForm.controls.name.setValue('');

    component.submit();

    expect(component.validationMessage).toContain('Complete');
  });
});

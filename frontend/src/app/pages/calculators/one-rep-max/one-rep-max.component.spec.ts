import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OneRepMaxComponent } from './one-rep-max.component';
import { provideRouter } from '@angular/router';

describe('OneRepMaxComponent', () => {
  let component: OneRepMaxComponent;
  let fixture: ComponentFixture<OneRepMaxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OneRepMaxComponent],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OneRepMaxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should calculate table weights as actual percentages of 1RM', () => {
    component.weight = 100;
    component.reps = 5;

    component.createTable();

    expect(component.calculateOneRepMax()).toBe(113);
    expect(component.table[1]).toEqual({ reps: 2, weight: 107, percentValue: 95 });
    expect(component.table[4]).toEqual({ reps: 5, weight: 90, percentValue: 80 });
  });

  it('should reject invalid repetition counts', () => {
    component.weight = 100;
    component.reps = 31;

    component.createTable();

    expect(component.table).toEqual([]);
    expect(component.errorMessage).toContain('between 1 and 30');
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WilksComponent } from './wilks.component';
import { provideRouter } from '@angular/router';

describe('WilksComponent', () => {
  let component: WilksComponent;
  let fixture: ComponentFixture<WilksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WilksComponent],
      providers: [provideRouter([])]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WilksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should produce the same score for equivalent kg and lb inputs', () => {
    component.gender = 'male';
    component.weight = 200;
    component.bodyweight = 80;
    component.calculateWilks();
    const metricScore = component.wilksScore;

    component.weight = 440.9245;
    component.bodyweight = 176.3698;
    component.weightUnit = 'lb';
    component.bodyweightUnit = 'lb';
    component.calculateWilks();

    expect(component.wilksScore).toBeCloseTo(metricScore, 2);
  });

  it('should report invalid inputs instead of calculating a score', () => {
    component.calculateWilks();

    expect(component.wilksScore).toBe(0);
    expect(component.errorMessage).toContain('Enter valid');
  });
});

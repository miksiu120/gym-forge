import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { CalculatorRecommendComponent } from './calculator-recommend.component';

describe('CalculatorRecommendComponent', () => {
  let component: CalculatorRecommendComponent;
  let fixture: ComponentFixture<CalculatorRecommendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CalculatorRecommendComponent],
      providers: [provideRouter([])],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CalculatorRecommendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

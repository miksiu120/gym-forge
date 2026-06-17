import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { CompleteTrainingComponent } from './complete-training.component';

describe('CompleteTrainingComponent', () => {
  let component: CompleteTrainingComponent;
  let fixture: ComponentFixture<CompleteTrainingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompleteTrainingComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(CompleteTrainingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => expect(component).toBeTruthy());
});

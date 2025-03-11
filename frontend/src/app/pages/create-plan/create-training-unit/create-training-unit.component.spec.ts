import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateTrainingUnitComponent } from './create-training-unit.component';

describe('CreateTrainingUnitComponent', () => {
  let component: CreateTrainingUnitComponent;
  let fixture: ComponentFixture<CreateTrainingUnitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateTrainingUnitComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateTrainingUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

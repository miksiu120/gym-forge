import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetDatesComponent } from './set-dates.component';

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
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

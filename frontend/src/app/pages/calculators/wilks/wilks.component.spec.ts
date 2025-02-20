import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WilksComponent } from './wilks.component';

describe('WilksComponent', () => {
  let component: WilksComponent;
  let fixture: ComponentFixture<WilksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WilksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WilksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

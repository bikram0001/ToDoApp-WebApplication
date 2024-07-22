import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActiveCompleteComponent } from './active-complete.component';

describe('ActiveCompleteComponent', () => {
  let component: ActiveCompleteComponent;
  let fixture: ComponentFixture<ActiveCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActiveCompleteComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ActiveCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

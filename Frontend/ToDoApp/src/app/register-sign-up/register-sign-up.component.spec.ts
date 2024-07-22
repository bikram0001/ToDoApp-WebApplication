import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterSignUpComponent } from './register-sign-up.component';

describe('RegisterSignUpComponent', () => {
  let component: RegisterSignUpComponent;
  let fixture: ComponentFixture<RegisterSignUpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterSignUpComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RegisterSignUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EncrypterComponent } from './encrypter.component';

describe('EncrypterComponent', () => {
  let component: EncrypterComponent;
  let fixture: ComponentFixture<EncrypterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EncrypterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EncrypterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

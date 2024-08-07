import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomGeneratorComponent } from './custom-generator.component';

describe('CustomGeneratorComponent', () => {
  let component: CustomGeneratorComponent;
  let fixture: ComponentFixture<CustomGeneratorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomGeneratorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomGeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplacerComponent } from './replacer.component';

describe('ReplacerComponent', () => {
  let component: ReplacerComponent;
  let fixture: ComponentFixture<ReplacerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReplacerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReplacerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

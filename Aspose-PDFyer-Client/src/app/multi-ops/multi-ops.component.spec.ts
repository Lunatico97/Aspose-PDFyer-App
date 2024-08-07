import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiOpsComponent } from './multi-ops.component';

describe('MultiOpsComponent', () => {
  let component: MultiOpsComponent;
  let fixture: ComponentFixture<MultiOpsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MultiOpsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiOpsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

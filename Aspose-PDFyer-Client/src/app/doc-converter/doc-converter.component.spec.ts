import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DocConverterComponent } from './doc-converter.component';

describe('DocConverterComponent', () => {
  let component: DocConverterComponent;
  let fixture: ComponentFixture<DocConverterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DocConverterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocConverterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

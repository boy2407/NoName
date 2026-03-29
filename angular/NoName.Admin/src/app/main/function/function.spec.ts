import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Function } from './function';

describe('Function', () => {
  let component: Function;
  let fixture: ComponentFixture<Function>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Function],
    }).compileComponents();

    fixture = TestBed.createComponent(Function);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

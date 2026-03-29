import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCategory } from './product-category';

describe('ProductCategory', () => {
  let component: ProductCategory;
  let fixture: ComponentFixture<ProductCategory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductCategory],
    }).compileComponents();

    fixture = TestBed.createComponent(ProductCategory);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

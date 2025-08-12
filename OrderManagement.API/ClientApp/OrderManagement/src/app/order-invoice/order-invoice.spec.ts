import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderInvoice } from './order-invoice';

describe('OrderInvoice', () => {
  let component: OrderInvoice;
  let fixture: ComponentFixture<OrderInvoice>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderInvoice]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderInvoice);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

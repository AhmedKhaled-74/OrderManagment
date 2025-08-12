import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { OrdersService } from '../Services/orders-service.js';
import {
  faPlus,
  faTrash,
  faCheck,
  faPlusCircle,
} from '@fortawesome/free-solid-svg-icons';
import { Item } from '../Models/item.js';
import { OrderItem } from '../Models/order-item.js';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { CommonModule } from '@angular/common';
import { OrderItemAdd } from '../Models/order-item-add.js';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-order',
  templateUrl: './new-order.html',
  imports: [FaIconComponent, ReactiveFormsModule, CommonModule],
})
export class NewOrder implements OnInit {
  orderForm: FormGroup;
  availableItems: Item[] = [];

  // Icons
  faPlus = faPlus;
  faTrash = faTrash;
  faCheck = faCheck;
  faPlusCircle = faPlusCircle;

  constructor(
    private fb: FormBuilder,
    private orderService: OrdersService,
    private _router: Router
  ) {
    this.orderForm = this.fb.group({
      customerName: ['', Validators.required],
      items: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.orderService
      .getItems()
      .subscribe((items) => (this.availableItems = items));
    this.addItem(); // Start with one item row
  }

  get items(): FormArray {
    return this.orderForm.get('items') as FormArray;
  }

  addItem(): void {
    this.items.push(
      this.fb.group({
        itemId: ['', Validators.required],
        quantity: [1, [Validators.required, Validators.min(1)]],
        unitPrice: [0],
      })
    );
  }

  removeItem(index: number): void {
    this.items.removeAt(index);
  }

  onItemSelect(index: number): void {
    const itemId = this.items.at(index).get('itemId')?.value;
    const selectedItem = this.availableItems.find(
      (item) => item.itemId === itemId
    );
    if (selectedItem) {
      this.items.at(index).patchValue({ unitPrice: selectedItem.itemPrice });
    }
  }

  updateTotal(index: number): void {
    const control = this.items.at(index);
    const quantity = control.get('quantity')?.value || 1;
    const price = control.get('unitPrice')?.value || 0;
    // no need to update anything, just ensure it's reflected in the view
  }

  getItemTotal(index: number): number {
    const item = this.items.at(index);
    const quantity = item.get('quantity')?.value || 1;
    const price = item.get('unitPrice')?.value || 0;
    return quantity * price;
  }

  onSubmit(): void {
    if (this.orderForm.invalid) return;

    const orderData = {
      customerName: this.orderForm.value.customerName,
    };

    const orderItems = this.items.value.map((item: any) => ({
      itemId: item.itemId,
      quantity: Math.max(1, Math.abs(Math.floor(Number(item.quantity))) || 1),
    }));

    // Transactional order + orderItems creation
    this.orderService.createOrder(orderData).subscribe((orderRes) => {
      if (!orderRes || !orderRes.orderId) {
        console.error('Order creation failed');
        return;
      }

      const orderId = orderRes.orderId;

      const itemRequests = orderItems.map((item: OrderItemAdd) => ({
        ...item,
        orderId,
      }));

      // Fire all order items and check for failure
      let errorOccurred = false;

      const requests = itemRequests.map((item: OrderItemAdd) =>
        this.orderService
          .createOrderItem(item)
          .toPromise()
          .catch((err) => {
            console.error('Order item creation failed:', err);
            errorOccurred = true;
            return null;
          })
      );

      Promise.all(requests).then((responses) => {
        if (errorOccurred) {
          console.error('Rolling back, not all order items succeeded.');
          // Optional: Call delete API to rollback the order
        } else {
          console.log('Order and items created successfully');
          this._router.navigateByUrl('orders');
        }
      });
    });
  }
}

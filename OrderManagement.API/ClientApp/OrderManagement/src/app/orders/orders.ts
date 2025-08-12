import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Order } from '../Models/order.js';
import { Observable } from 'rxjs';
import { OrdersService } from '../Services/orders-service.js';
import { Router } from '@angular/router';
import { faReceipt } from '@fortawesome/free-solid-svg-icons';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-orders',
  imports: [CommonModule, FaIconComponent],
  templateUrl: './orders.html',
  styleUrl: './orders.scss',
})
export class Orders implements OnInit {
  faReceipt = faReceipt;
  orders$!: Observable<Order[]>;
  constructor(private _ordersservice: OrdersService, private _router: Router) {}
  ngOnInit(): void {
    this.loadOrders();
  }
  loadOrders() {
    this.orders$ = this._ordersservice.getOrders();
  }

  viewInvoice(id: string): void {
    this._router.navigateByUrl(`orders/${id}`);
  }
}

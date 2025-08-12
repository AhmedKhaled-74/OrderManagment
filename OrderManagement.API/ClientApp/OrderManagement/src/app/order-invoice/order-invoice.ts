import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../Services/orders-service.js';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from '../Models/order.js';
import { CommonModule, Location } from '@angular/common';
import * as bootstrap from 'bootstrap';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import {
  faArrowLeft,
  faDeleteLeft,
  faPrint,
  faTrashAlt,
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-order-invoice',
  imports: [CommonModule, FaIconComponent],
  templateUrl: './order-invoice.html',
  styleUrl: './order-invoice.scss',
})
export class OrderInvoice implements OnInit {
  faArrowLeft = faArrowLeft;
  faDeleteLeft = faDeleteLeft;
  faPrint = faPrint;
  faTrashAlt = faTrashAlt;

  deleteError = false;
  orderId: string = '';
  order$!: Observable<Order>;
  constructor(
    private _ordersService: OrdersService,
    private _route: ActivatedRoute,
    private location: Location,
    private _router: Router
  ) {}
  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.orderId = params['id']; // The '+' converts the string to a number
      this.order$ = this._ordersService.getOrderById(this.orderId);
    });
  }
  async confirmDelete() {
    if (!this.orderId) return;

    try {
      // 1. Get modal element and Bootstrap reference early
      const modalEl = document.getElementById('deleteConfirmModal');
      const bootstrap = await import('bootstrap');
      const modal = modalEl ? bootstrap.Modal.getInstance(modalEl) : null;

      // 2. Hide modal immediately
      modal?.hide();

      // 3. Delete the order
      this._ordersService.deleteOrderById(this.orderId).subscribe({
        next: () => {
          // 4. Force cleanup after animation completes
          setTimeout(() => this.forceCleanupModals(), 350); // Slightly longer than Bootstrap's fade duration

          this._router.navigate(['/orders']);
        },
        error: (err) => {
          console.error('Delete failed:', err);
          this.deleteError = true;
          this.forceCleanupModals(); // Cleanup even on error
        },
      });
    } catch (e) {
      console.error('Modal handling error:', e);
      this.forceCleanupModals();
      this.deleteError = true;
    }
  }

  private forceCleanupModals() {
    if (typeof document === 'undefined') return;

    // Remove all modal backdrops
    document.querySelectorAll('.modal-backdrop').forEach((el) => el.remove());

    // Remove modal-open class
    document.body.classList.remove('modal-open');

    // Reset scroll
    document.body.style.overflow = '';
    document.body.style.paddingRight = '';
  }
  goBack(): void {
    this.location.back();
  }

  printOrder(order: Order): void {
    const printWindow = window.open('', '_blank');
    if (printWindow) {
      printWindow.document.write(`
      <html>
        <head>
          <title>Print Order #${order.orderNumber}</title>
          <style>
            body { font-family: Arial, sans-serif; padding: 20px; }
            table { width: 100%; border-collapse: collapse; margin-top: 10px; }
            th, td { padding: 8px; border: 1px solid #ccc; text-align: left; }
            th { background-color: #f5f5f5; }
          </style>
        </head>
        <body>
          <h1>Order #${order.orderNumber}</h1>
          <p><strong>Date:</strong> ${new Date(
            order.orderDate
          ).toLocaleString()}</p>
          <p><strong>Customer:</strong> ${order.customerName}</p>
          <table>
            <thead>
              <tr>
                <th>Item</th>
                <th>Qty</th>
                <th>Unit Price</th>
                <th>Total</th>
              </tr>
            </thead>
            <tbody>
              ${order.orderItems
                .map(
                  (item) => `
                <tr>
                  <td>${item.itemName}</td>
                  <td>${item.quantity}</td>
                  <td>$${item.unitPrice.toFixed(2)}</td>
                  <td>$${(item.quantity * item.unitPrice).toFixed(2)}</td>
                </tr>
              `
                )
                .join('')}
            </tbody>
          </table>
          <p><strong>Total:</strong> $${order.totalAmount.toFixed(2)}</p>
        </body>
      </html>
    `);
      printWindow.document.close();
      printWindow.focus();
      printWindow.print();
    }
  }
}

import { OrderItem } from './order-item.js';

export interface Order {
  orderId: string;
  orderNumber: string;
  orderDate: string;
  customerName: string;
  totalAmount: number;
  orderItems: OrderItem[];
}

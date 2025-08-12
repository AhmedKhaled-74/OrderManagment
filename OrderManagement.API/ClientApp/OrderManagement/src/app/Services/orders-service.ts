import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, Observable, of, throwError } from 'rxjs';
import { Order } from '../Models/order.js';
import { Item } from '../Models/item.js';
import { OrderItemAdd } from '../Models/order-item-add.js';
import { OrderAdd } from '../Models/order-add.js';
import { OrderItem } from '../Models/order-item.js';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  getOrdersURL = 'https://localhost:7143/api/v1/Orders';
  getItemsURL = 'https://localhost:7143/api/v1/Items';
  getOrderItemsURL = 'https://localhost:7143/api/v1/OrderItems';
  token = localStorage.getItem('token') || sessionStorage.getItem('token');

  constructor(private _http: HttpClient) {}
  getOrders(): Observable<Order[]> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);
    console.log(headers.get('Authorization'));

    return this._http
      .get<Order[]>(this.getOrdersURL, { headers: headers })
      .pipe(
        catchError((err) => {
          console.error(err);
          return of([]); // return empty list on error
        })
      );
  }
  getOrderById(id: string): Observable<Order> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);

    return this._http
      .get<Order>(`${this.getOrdersURL}/${id}`, { headers: headers })
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(); // return empty list on error
        })
      );
  }
  deleteOrderById(id: string) {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);
    return this._http
      .delete<Order>(`${this.getOrdersURL}/${id}`, { headers: headers })
      .pipe(
        catchError((err) => {
          console.error('Delete error in service:', err);
          return throwError(() => err); // <== re-throw so component can catch it
        })
      );
  }

  getItems(): Observable<Item[]> {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);
    return this._http.get<Item[]>(this.getItemsURL, { headers: headers }).pipe(
      catchError((err) => {
        console.error(err);
        return of([]); // return empty list on error
      })
    );
  }

  createOrderItem<OrderItem>(orderAdd: OrderItemAdd) {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);
    return this._http
      .post<OrderItem>(this.getOrderItemsURL, orderAdd, { headers: headers })
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(null); // return empty list on error
        })
      );
  }
  createOrder(orderAdd: OrderAdd) {
    let headers = new HttpHeaders();
    headers = headers.append('Authorization', `Bearer ${this.token}`);
    return this._http
      .post<Order>(this.getOrdersURL, orderAdd, { headers: headers })
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(null); // return empty list on error
        })
      );
  }
}

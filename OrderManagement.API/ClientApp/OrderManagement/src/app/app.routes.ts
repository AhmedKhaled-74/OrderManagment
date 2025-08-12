import { Routes } from '@angular/router';
import { Notfound } from './notfound/notfound.js';
import { Orders } from './orders/orders.js';
import { OrderInvoice } from './order-invoice/order-invoice.js';
import { NewOrder } from './new-order/new-order.js';
import { Register } from './register/register.js';
import { Login } from './login/login.js';
import { authGuard } from './auth-guard.js';

export const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  { path: 'account/register', component: Register },
  { path: 'account/login', component: Login },

  { path: 'orders', component: Orders, canActivate: [authGuard] },
  { path: 'orders/create', component: NewOrder, canActivate: [authGuard] },
  { path: 'orders/:id', component: OrderInvoice, canActivate: [authGuard] },

  { path: '**', component: Notfound },
];

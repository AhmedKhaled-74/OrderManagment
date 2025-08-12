import { RegisterDTO } from '../Models/register-dto';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { User } from '../Models/user.js';
import { LoginDTO } from '../Models/login-dto.js';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  public currentUserName: string | null = null;

  constructor(private _http: HttpClient) {}
  AccountURL = 'https://localhost:7143/api/v1/Account';

  checkEmailExists(email: string): Observable<boolean> {
    return this._http.get<boolean>(
      `${this.AccountURL}/register/check-email?email=${encodeURIComponent(
        email
      )}`
    );
  }

  Registeration(registerDTO: RegisterDTO) {
    return this._http
      .post<User>(`${this.AccountURL}/Register`, registerDTO)
      .pipe(
        catchError((err) => {
          console.error(err);
          return of(err); // return empty list on error
        })
      );
  }
  generateNewToken(): Observable<any> {
    var token =
      localStorage.getItem('token') || sessionStorage.getItem('token');
    var refreshToken =
      localStorage.getItem('refreshToken') ||
      sessionStorage.getItem('refreshToken');
    return this._http.post<any>(`${this.AccountURL}/generate-new-token`, {
      token: token,
      refreshToken: refreshToken,
    });
  }
  getAccessToken(): string | null {
    return localStorage.getItem('token') || sessionStorage.getItem('token');
  }
  Login(loginDTO: LoginDTO) {
    return this._http.post<User>(`${this.AccountURL}/Login`, loginDTO).pipe(
      catchError((err) => {
        console.error(err);
        return of(err); // return empty list on error
      })
    );
  }
  LogOut() {
    return this._http.get(`${this.AccountURL}/logout`).pipe(
      catchError((err) => {
        console.error(err);
        return of(err); // return empty list on error
      })
    );
  }
}

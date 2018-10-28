import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginViewModel } from '../viewmodels/login.viewmodel';
import { Observable } from 'rxjs';
import { AuthorizedUserInfoViewModel } from 'src/app/viewmodels/authorizeduser.viewmodel';
import { Response, Http } from '@angular/http';
import { map, tap } from 'rxjs/operators';
import { Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';

interface IAuthService {
  login(userdata: LoginViewModel);
  logout(): void;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService implements IAuthService {
  public isLoggedIn = false;
  public redirectUrl: string;
  private readonly authUrl = 'http://localhost:4442/api/auth/login';
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'my-auth-token'
    })
  };

  constructor(private http: HttpClient,
    private router: Router) {
  }

  public login(userdata: LoginViewModel) {
    return this.http.post<AuthorizedUserInfoViewModel>(this.authUrl, JSON.stringify(userdata), this.httpOptions)
      .pipe(
          map(user => {
          if (user && user.token) {
            localStorage.setItem('currentUser', JSON.stringify(user));
          }
      }),
          tap(val => {
            this.isLoggedIn = true;
            this.router.navigate(['/']);
          }));
  }

  public logout() {
    localStorage.removeItem('currentUser');
    this.isLoggedIn = false;
    this.router.navigate(['/']);
  }


  private getRequestOptions() {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    headers.append('Access-Control-Allow-Origin', '*');
    return headers;
  }
}

import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LoginViewModel } from 'src/app/viewmodels/login.viewmodel';
import { AuthorizedUserInfoViewModel } from 'src/app/viewmodels/authorizeduser.viewmodel';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [AuthService]
})
export class LoginComponent implements OnInit {
  private isLoggedIn: boolean;
  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router) {

      this.form = this.formBuilder.group({
        username: ['', Validators.required],
        password: ['', Validators.required]
      });
    }

    login() {
      const val = this.form.value;

      if (val.username && val.password) {
        this.authService.login(new LoginViewModel(val.username, val.password))
        .subscribe();
      }
    }

    logout() {
      this.authService.logout();
    }

  ngOnInit() {
    this.isLoggedIn = localStorage.getItem('currentUser') !== null;
  }

}

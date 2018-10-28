import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from '../app/login/login.component';
import { AdminComponent } from '../app/admin/admin.component';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { HomeComponent } from 'src/app/home/home.component';

const routes: Routes = [
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'admin', component: AdminComponent,
    // canActivate: [AuthGuard]
   },
   {
    path: 'home', component: HomeComponent
   },
   {
     path: '',
     redirectTo: '/home',
     pathMatch: 'full'
   }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

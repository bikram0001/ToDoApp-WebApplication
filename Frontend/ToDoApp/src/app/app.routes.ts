import { Routes } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HomeComponent } from './home/home.component';
import { authGuard } from './guards/authGuard';
import { ActiveCompleteComponent } from './active-complete/active-complete.component';
import { RegisterSignUpComponent } from './register-sign-up/register-sign-up.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
export const routes: Routes = [
    {path:'',redirectTo:'/login',pathMatch:'full'},
    {path:'login',component:RegisterSignUpComponent},
    {path:'signUp',component:RegisterSignUpComponent},
    {path:'home',component:HomeComponent,children: [
        {path: '', redirectTo : 'dashboard',pathMatch:'full'},
        {path:'dashboard',component:DashboardComponent, canActivate : [authGuard]},
        {path:'active',component:ActiveCompleteComponent, canActivate : [authGuard]},
        {path:'completed',component:ActiveCompleteComponent, canActivate : [authGuard]},
    ]},
    {path:'**',component:PageNotFoundComponent},
];
import {
  HttpRequest,
  HttpEvent,
  HttpHandlerFn,
  HttpErrorResponse
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, catchError, finalize, switchMap, throwError } from 'rxjs';
import { navigationRoute } from '../enums/routeNames';
import { SpinnerService } from './spinnerService';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserServices } from './userServices';
import { TokenResponse } from '../models/tokenResponse';
export function AuthInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
    var spinnerService = inject(SpinnerService);
    var userService = inject(UserServices);
    spinnerService.showSpinner();
    const token = localStorage.getItem('Token') as string;
    const refreshToken = localStorage.getItem('RefreshToken') as string;
    var helper=new JwtHelperService();
    var newReq = req;
    const isAccessTokenExpired = helper.isTokenExpired(token);
    const isRefreshTokenExpired = helper.isTokenExpired(refreshToken);
    if(isAccessTokenExpired && isRefreshTokenExpired){
      redirectToLogin();
    }
    if(token!=null){
    newReq = req.clone({setHeaders: {
                                Authorization: `Bearer ${token}`,
                                'Content-Type' : 'application/json'
                            }});
    }
    return next(newReq).pipe(catchError((error : any)=>{
      if(error instanceof HttpErrorResponse){
        if(error.status == 401){
          return userService.getTokens(refreshToken).pipe(
            switchMap((response : TokenResponse)=>{
              localStorage.setItem('Token',response.token);
              localStorage.setItem('RefreshToken',response.refreshToken);
              newReq = req.clone({setHeaders: {
                Authorization: `Bearer ${response.token}`,
                'Content-Type' : 'application/json'
              }});
              return next(newReq);
            }),catchError((err:HttpErrorResponse)=>{
              if(err.status == 401){
                redirectToLogin();
              }
              return throwError(()=> err)
            })
          );
        }
      }
      return throwError(()=>error);
    }),
    finalize(() => {
      spinnerService.hideSpinner();
    }));
}
function redirectToLogin(){
  var router = inject(Router);
  router.navigate([navigationRoute.login]);
  localStorage.removeItem('Token');
  localStorage.removeItem('RefreshToken');
}
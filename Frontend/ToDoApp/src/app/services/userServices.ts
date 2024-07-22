import { HttpClient, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { User } from "../models/user";
import { catchError, throwError } from "rxjs";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { apiEndPoints } from "../constants/apiEndPoints";
import { constants } from "../constants/constants";
import { TokenResponse } from "../models/tokenResponse";

@Injectable({
    providedIn : 'root'
})
export class UserServices{
    constructor(private http: HttpClient,private router : Router,private toastr : ToastrService){}
    userSignUp(userName : string, password : string){
        var user : User = {
            userName : userName,
            password : password
        }
        return this.http.post(apiEndPoints.registerUser,user);
    }
    userLogin(userName : string, password : string){
        var user : User = {
            userName : userName,
            password : password
        }
        return this.http.post<TokenResponse>(apiEndPoints.loginUser,user).pipe(catchError((error : any)=>{
            if(error instanceof HttpErrorResponse){
              if(error.status == 401){
                this.toastr.error(constants.invalidUserDetailsToastrMsg);
              }
            }
            return throwError(()=>error);
          }));
    }
    userExists(userName : string){
        return this.http.get<boolean>(apiEndPoints.checkUserExists(userName));
    }
    getTokens(refreshToken : string){
        return this.http.post<TokenResponse>(apiEndPoints.getTokens,`\"${refreshToken}\"`);
    }
}
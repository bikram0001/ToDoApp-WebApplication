import { NgClass, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserServices } from '../services/userServices';
import { ToastrService } from 'ngx-toastr';
import { navigationRoute } from '../enums/routeNames';
import { constants } from '../constants/constants';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [NgClass,ReactiveFormsModule,NgIf],
  templateUrl: './register-sign-up.component.html',
  styleUrl: './register-sign-up.component.css'
})
export class RegisterSignUpComponent implements OnInit{
  hidePassword : Boolean = true;
  signIn: Boolean =false;
  registerLoginForm! : FormGroup;
  buttonContent : string =  constants.signUp;
  signInText : string = constants.signUpSubText;
  eyeImg : string = constants.hidePasswordImage;
  constructor(private router:Router,private _userService : UserServices,private toastr : ToastrService) {}
  ngOnInit(): void {
    this.defaultLogin();
    if(this.router.url == navigationRoute.login){
      this.signIn = true;
      this.buttonContent = constants.signIn;
      this.signInText = constants.signInSubText;
    }
    this.defualtFormControls();
  }

  defualtFormControls(){
    this.registerLoginForm = new FormGroup({
      userName : new FormControl(null,[Validators.required,Validators.minLength(3),Validators.pattern(/^[a-zA-Z0-9_]{3,20}$/)]),
      password : new FormControl(null,[Validators.required,Validators.minLength(3),Validators.pattern(/^[a-zA-Z0-9_]{3,20}$/)])
    })
  }

  toogleSignInSignUp(){
    if(this.signIn){
      this.router.navigate([navigationRoute.signUp]);
    }
    else{
      this.router.navigate([navigationRoute.login]);
    }
    this.signIn = !this.signIn;
  }
  
  formSubmit(){
    // Sign in 
    if(this.signIn){
      if(this.registerLoginForm.valid)
      {
        this._userService.userLogin(this.registerLoginForm.value.userName,this.registerLoginForm.value.password).subscribe({
                next : (response)=>{
                  if (response && response.token){
                    localStorage.setItem('Token',response.token);
                    localStorage.setItem('RefreshToken',response.refreshToken);
                    this.router.navigate([navigationRoute.dashboard]);
                    this.toastr.success(constants.loginSuccessToastrMsg);
                  }
                  else{
                    this.toastr.error(constants.UserNamePasswordToastrMsg);
                  }
                },
                error : () => {
                  this.toastr.error(constants.errorToastrMsg);
                }
              }
            );
      }
      else{
        this.toastr.error(constants.invalidUserDetailsToastrMsg);
      }
    }
    // Sign up
    else{
      if(this.registerLoginForm.valid){
        this._userService.userExists(this.registerLoginForm.value.userName).subscribe({
            next : (response)=>{
              if(response){
                this.toastr.error(constants.userExistsToastrMsg);
              }
              else{
                this._userService.userSignUp(this.registerLoginForm.value.userName,this.registerLoginForm.value.password).subscribe(
                  ()=>{
                    this.router.navigate([navigationRoute.login]);
                    this.toastr.success(constants.userAddedToastrMsg);
                  }
                );
              }
            },
            error : ()=>{
              this.toastr.error(constants.errorToastrMsg);
            }
          }
        )
      }
      else{
        this.toastr.info(constants.UserNamePasswordToastrMsg);
      }
    }
  }
  defaultLogin(){
    var token = localStorage.getItem('Token');
    if(token!=null){
      this.router.navigate([navigationRoute.dashboard]);
    }
  }
  toogleHide(){
    this.hidePassword = !this.hidePassword;
    this.eyeImg = this.hidePassword ? constants.hidePasswordImage : constants.showPasswordImage;
  }
}

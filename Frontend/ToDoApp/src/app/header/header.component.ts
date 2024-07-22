import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserServices } from '../services/userServices';
import { navigationRoute } from '../enums/routeNames';
import { taskCategory } from '../enums/taskCategory';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [FormsModule,RouterLink],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit{
  @Output() addTaskEvent = new EventEmitter();
  @Output() headerEvent = new EventEmitter();
  @Input() heading! : string;
  constructor(private router : Router,private userService : UserServices) {}
  ngOnInit(): void {
    if(this.router.url == navigationRoute.active){
      this.heading = taskCategory.Active;
    }
    else if(this.router.url == navigationRoute.completed){
      this.heading = taskCategory.Completed;
    }
    else{
      this.heading = taskCategory.Dashboard;
    }
  }
  
  signOut(){
    localStorage.removeItem("Token");
    localStorage.removeItem("RefreshToken");
    this.router.navigate([navigationRoute.login]);
  }

  // mobile view functions
  showAddTask(){
    this.addTaskEvent.emit(true); 
  }
  changeRoute(event : any){
    if(event.target.value == taskCategory.Dashboard){
      this.router.navigate([navigationRoute.dashboard])
    }
    else if(event.target.value == taskCategory.Active){
      this.router.navigate([navigationRoute.active]);
    }
    else{
      this.router.navigate([navigationRoute.completed]);
    }
  }
}

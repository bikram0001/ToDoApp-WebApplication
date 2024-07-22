import { Component, OnDestroy, OnInit } from '@angular/core';
import { SideNavBarComponent } from "../side-nav-bar/side-nav-bar.component";
import { HeaderComponent } from "../header/header.component";
import { RouterOutlet } from '@angular/router';
import { AddTaskComponent } from '../add-task/add-task.component';
import { NgClass, NgIf } from '@angular/common';
import { SharedDataService } from '../services/sharedDataService';
import { taskCategory } from '../enums/taskCategory';

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [RouterOutlet, SideNavBarComponent, HeaderComponent,AddTaskComponent,NgClass,NgIf]
})
export class HomeComponent implements OnInit{
    headerHeading : string = taskCategory.Dashboard;
    addTaskVisible : Boolean = false;
    constructor(private _sharedService : SharedDataService) {}
    ngOnInit(): void {
        this._sharedService.getData().subscribe(
            (response)=>{
                if(response){this.addTaskVisible = true;}
            }
        );
    }
    addTask(event : any){
        this.addTaskVisible = event;
    }
    hideTask(event: any){
        this.addTaskVisible = event;
    }
    changeHeader(heading : string){
        this.headerHeading=heading;
    }
}

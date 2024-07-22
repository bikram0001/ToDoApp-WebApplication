import { NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { TaskServices } from '../services/taskServices';
import { ToastrService } from 'ngx-toastr';
import { constants } from '../constants/constants';
import { TaskResponse } from '../models/taskResponse';
import { kpi } from '../models/kpi';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgFor,DatePipe,NgClass,NgIf],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit{
  tasksPercentage : kpi = {activeTasks : 0,completedTasks : 0};
  tasks : TaskResponse[] = [];
  currentDate = new Date();
  activePercentage : number = 0;
  completedPercentage : number = 0;
  dataUnavialable : Boolean = true;

  constructor(private _taskService : TaskServices,private toastr : ToastrService) {}

  ngOnInit(): void {
    this.getTasksData();
    this._taskService.reloadObservable.subscribe(
      ()=>{
        this.getTasksData();
      }
    );
  }
  
  getTasksData(){
    this._taskService.getTasksData().subscribe(
      (response)=>{
        this.tasks = response;
        this.dataUnavialable = this.tasks.length > 0 ? false : true;
        this.performanceIndicator();
      }
    );
  }

  performanceIndicator(){
    this._taskService.performanceIndicator().subscribe(
      (response)=>{
        this.tasksPercentage.activeTasks=response.activeTasks,
        this.tasksPercentage.completedTasks=response.completedTasks
      }
    );
  }
  
  deleteTasks(){
    if(this.tasks.length>0 && confirm("Confirm to delete all the tasks")){
      this._taskService.deleteTasks().subscribe(
        ()=>{
          this.tasks=[];
          this.performanceIndicator();
          this.toastr.success(constants.tasksdeleteToastrMsg);
        }
      );
    }
  }
}

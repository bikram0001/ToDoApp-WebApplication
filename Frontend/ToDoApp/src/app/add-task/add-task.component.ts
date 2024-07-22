import { Component, EventEmitter, OnInit, Output, output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskServices } from '../services/taskServices';
import { SharedDataService } from '../services/sharedDataService';
import { ToastrService } from 'ngx-toastr';
import { constants } from '../constants/constants';
import { TaskResponse } from '../models/taskResponse';

@Component({
  selector: 'app-add-task',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent implements OnInit{
  @Output() addTaskEvent = new EventEmitter<Boolean>();
  showAddTask : Boolean = false;
  editTask : Boolean = false;
  addTask! : FormGroup;
  data : TaskResponse | null = null;
  id : number = 0;
  constructor(private _taskService : TaskServices,private _sharedService : SharedDataService,private toastr : ToastrService) {}
  ngOnInit(): void {
    this.addTask = new FormGroup({
      taskTitle : new FormControl(null,[Validators.required,Validators.pattern(/^[^\s].*/)]),
      taskDescription : new FormControl(null)
    });
    this._sharedService.getData().subscribe(
      (response)=>{
        if(response){
            this.addTask.patchValue({
              taskTitle: response.title,
              taskDescription: response.description
            });
            this.editTask = true;
            this.id = response.id;
        }
      }
    );
  }
  hideAddTask(){
    this._sharedService.clearData();
    this.editTask=false;
    this.addTaskEvent.emit(this.showAddTask);
  }
  saveTask(){
    if(this.addTask.valid){
      if(!this.editTask){
        this._taskService.addTaskData(this.addTask.value.taskTitle,this.addTask.value.taskDescription).subscribe(
          ()=>{
            this._taskService.triggerReload();
            this.toastr.success(constants.taskAddToastrMsg);
          }
        );
      }
      else{
        this._taskService.updateTask(this.addTask.value.taskTitle,this.addTask.value.taskDescription,this.id).subscribe(
          ()=>{
            this._taskService.triggerReload();
            this.toastr.success(constants.taskEditToastrMsg);
          }
        )
        this.editTask = false;
      }
      this._sharedService.clearData();
      this.addTaskEvent.emit(this.showAddTask);
    }
    else if(this.addTask.value.taskTitle == null){
      this.toastr.warning(constants.titleDescToastrMsg);
    }
    else{
      this.toastr.warning(constants.leadingSpaceTitle);
    }
  }
}

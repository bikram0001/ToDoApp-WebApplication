import { DatePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TaskServices } from '../services/taskServices';
import { SharedDataService } from '../services/sharedDataService';
import { TimeDiffPipe } from '../pipes/timeDiff';
import { ToastrService } from 'ngx-toastr';
import { taskStatus } from '../enums/taskStatus';
import { constants } from '../constants/constants';
import { TaskResponse } from '../models/taskResponse';
@Component({
    selector: 'app-active',
    standalone: true,
    templateUrl: './active-complete.component.html',
    styleUrl: './active-complete.component.css',
    imports: [DatePipe, NgFor, NgClass, NgIf, TimeDiffPipe]
})
export class ActiveCompleteComponent implements OnInit{
  currentDate : Date = new Date();
  dataUnavialable : Boolean = true;
  heading : string = constants.activeTaskHeading;
  completed : Boolean = false;
  currentRoute? : string;
  checkBoxImage : string = constants.activeCheckboxImage;
  binImage : string = constants.activeBinImage;
  descriptions: Boolean[] = new Array(0).fill(false);
  prevDesc:number = -1;
  taskData : TaskResponse[] = [];
  update : string = taskStatus.completed;
  constructor(private route : ActivatedRoute,private _taskService : TaskServices,private _sharedService : SharedDataService,private toastr : ToastrService) {}
  ngOnInit(): void {
    this.currentRoute=this.route.snapshot.url[this.route.snapshot.url.length - 1]?.path;
    if(this.currentRoute==taskStatus.completed){
      this.update = taskStatus.active;
      this.heading = constants.completeTaskHeading;
      this.completed = true;
      this.checkBoxImage = constants.completedCheckBoxImage;
      this.binImage = constants.completedBinImage;
      this.getCompletedTasks();
    }
    else{
      this.getActiveTasks();
    }
    this._taskService.reloadObservable.subscribe(
      ()=>{
        this.currentRoute==taskStatus.completed ? this.getCompletedTasks() : this.getActiveTasks();
      }
    );
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const clickedInside = (event.target as HTMLElement).closest('.task,.description-container');
    if (!clickedInside) {
      this.descriptions.fill(false);
    }
  }
  toogleDescription(index:number){
    if(this.prevDesc!=-1 && index!=this.prevDesc){
      this.descriptions[this.prevDesc]=false;
    }
    this.descriptions[index]=!this.descriptions[index];
    this.prevDesc = index==this.prevDesc ? -1 : index;
  }
  getActiveTasks(){
    this._taskService.getActiveTasksData().subscribe(
      (response)=>{
        this.taskData = response;
        this.generateDescArray();
        this.dataUnavialable = this.taskData.length >0 ?  false : true;
      }
    );
  }
  getCompletedTasks(){
    this._taskService.getCompletedTasksData().subscribe(
      (response)=>{
        this.taskData = response;
        this.generateDescArray();
        this.dataUnavialable = this.taskData.length >0 ?  false : true;
      }
    );
  }
  deleteTask(id : number,index : number){
    if(confirm(constants.taskDeletePopUpMsg)){
      this._taskService.deleteTask(id).subscribe(
        ()=>{
          this.taskData.splice(index,1);
          this.generateDescArray();
          this.dataUnavialable = this.taskData.length >0 ?  false : true;
          this.toastr.success(constants.taskDeleteToastrMsg);
        }
      );
    }
  }
  generateDescArray(){
    this.prevDesc = -1;
    var count = this.taskData.length;
    this.descriptions = new Array(count).fill(false);
  }
  changeTaskStatus(task : TaskResponse){
    this._taskService.changeTaskStatus(task.id).subscribe(
      ()=>{
        this.taskData = this.taskData.filter(x => x !== task);
      }
    );
  }
  editTask(task: TaskResponse){
    this._sharedService.storeData(task);
  }
  showConfirmPopup(task : TaskResponse,index : number){
    if(confirm(constants.taskUpdatePopUpMsg(this.update))){
      this.confirmTask(task);
    }
  }
  confirmTask(task : TaskResponse){
    this.toogleDescription(this.prevDesc);
    this.changeTaskStatus(task);
    this.toastr.success(constants.taskUpdateToastrMsg(this.update));
  }
}

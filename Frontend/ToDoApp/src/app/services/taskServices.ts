import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { apiEndPoints } from "../constants/apiEndPoints";
import { TaskResponse } from "../models/taskResponse";
import { TaskRequest } from "../models/taskRequest";
import { kpi } from "../models/kpi";

@Injectable({
    providedIn:'root',
})
export class TaskServices{
    constructor(private http: HttpClient){}

    private reloadSubject = new Subject<void>();
    get reloadObservable(){
        return this.reloadSubject.asObservable();
    }
    triggerReload(){
        this.reloadSubject.next();
    }

    getTasksData(){
        return this.http.get<TaskResponse[]>(apiEndPoints.allTasks);
    }
    getActiveTasksData(){
        return this.http.get<TaskResponse[]>(apiEndPoints.activeTasks);
    }
    getCompletedTasksData(){
        return this.http.get<TaskResponse[]>(apiEndPoints.completedTasks);
    }
    deleteTask(id : number){
        return this.http.delete(apiEndPoints.deleteTask(id));
    }
    deleteTasks(){
        return this.http.delete(apiEndPoints.deleteTasks);
    }
    addTaskData(title : string ,description : string){
        var task : TaskRequest = {
            title : title,
            description : description
        };
        return this.http.post(apiEndPoints.addTask,task);
    }
    changeTaskStatus(id : number){
        return this.http.put(apiEndPoints.changeTaskStatus(id),null);
    }
    updateTask(title : string, description : string ,id : number){
        var task : TaskRequest = {
            title : title,
            description : description
        };
        return this.http.put(apiEndPoints.updateTask(id),task);
    }
    performanceIndicator(){
        return this.http.get<kpi>(apiEndPoints.tasksPercentage);
    }
}
import { Injectable } from "@angular/core";
import { ReplaySubject } from "rxjs";
import { TaskResponse } from "../models/taskResponse";

@Injectable({
    providedIn: 'root',
  })
  export class SharedDataService {
    private _data : ReplaySubject<TaskResponse | null> = new ReplaySubject<TaskResponse | null>(1);
      
    storeData(data : TaskResponse | null){
        this._data.next(data);
    }
    getData(){
        return this._data.asObservable();
    }
    clearData(){
        this._data.next(null);
    }
  }
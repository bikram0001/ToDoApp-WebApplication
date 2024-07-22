import { Injectable } from "@angular/core";
import { Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
  })
  export class SpinnerService {
    private spinnerSubject = new Subject<boolean>();
    spinnerVisibility = this.spinnerSubject.asObservable();
    showSpinner() { 
      this.spinnerSubject.next(true);
    }
    hideSpinner() {
      this.spinnerSubject.next(false);
    }
  }
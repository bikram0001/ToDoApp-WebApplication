import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SpinnerService } from './services/spinnerService';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [RouterOutlet,NgIf]
})
export class AppComponent{
  title = 'ToDoApp';
  showSpinner : Boolean = false;
  constructor(private _spinnerService : SpinnerService) {}
  ngAfterViewInit(): void{
    this._spinnerService.spinnerVisibility.subscribe(
      (response)=>{
        setTimeout(() => {
          this.showSpinner = response;
        });
      }
    )
  }
}

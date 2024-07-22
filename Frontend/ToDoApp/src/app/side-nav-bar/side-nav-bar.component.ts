import { Component, EventEmitter, Output, output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-side-nav-bar',
  standalone: true,
  imports: [RouterLink,RouterLinkActive],
  templateUrl: './side-nav-bar.component.html',
  styleUrl: './side-nav-bar.component.css'
})
export class SideNavBarComponent {
  addTaskClicked : Boolean = true;
  @Output() addTaskEvent = new EventEmitter();
  @Output() headerEvent = new EventEmitter();
  showAddTask(){
    this.addTaskEvent.emit(this.addTaskClicked);
  }
  changeHeader(heading : string){
    this.headerEvent.emit(heading);
  }
}

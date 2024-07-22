import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeDiff',
  standalone : true
})
export class TimeDiffPipe implements PipeTransform {
  transform(createdOnDate: Date): string {
    const createdOn = new Date(createdOnDate);
    const now = new Date();
    const diffInMs = now.getTime() - createdOn.getTime();
    const diffInDays = Math.floor(diffInMs / (1000 * 60 * 60 * 24));
    if(diffInDays!=0){
        return `${diffInDays} Days ago`;
    }
    const diffInHours = Math.floor(diffInMs / (1000 * 60 * 60));
    if(diffInHours!=0){
        return `${diffInHours} hours ago`;
    }
    const diffInMin = Math.floor(diffInMs / (1000 * 60));
    if(diffInMin>0){return `${diffInMin} minutes ago`;}
    return 'just now';
  }
}
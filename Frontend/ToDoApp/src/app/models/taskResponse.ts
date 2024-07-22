export interface TaskResponse{
    id : number,
    taskId : number,
    title : string,
    description : string, 
    status : string,
    createdOn : Date,
    completedOn : Date
}
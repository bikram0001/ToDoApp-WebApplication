import { environment } from "../environment/enviroment"

var baseUrl = environment.baseUrl;
export const apiEndPoints = {
    allTasks: `${baseUrl}/api/Task`,
    activeTasks: `${baseUrl}/api/Task/Active`,
    completedTasks: `${baseUrl}/api/Task/Completed`,
    deleteTask: (id: number) => `${baseUrl}/api/Task/Task?id=${id}`,
    deleteTasks: `${baseUrl}/api/Task`,
    addTask: `${baseUrl}/api/Task`,
    changeTaskStatus: (id: number) => `${baseUrl}/api/Task?id=${id}`,
    updateTask: (id: number) => `${baseUrl}/api/Task/Update?id=${id}`,
    registerUser: `${baseUrl}/api/Auth/adduser`,
    loginUser: `${baseUrl}/api/Auth`,
    checkUserExists: (userName: string) => `${baseUrl}/api/Auth?userName=${userName}`,
    tasksPercentage : `${baseUrl}/api/Task/kpi`,
    getTokens : `${baseUrl}/api/Auth/tokens`,
}
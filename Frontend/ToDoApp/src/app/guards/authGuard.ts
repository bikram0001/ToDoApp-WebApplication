import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { navigationRoute } from '../enums/routeNames';

export const authGuard: CanActivateFn = (route, state) => {
    var token = localStorage.getItem('Token');
    var router : Router = inject(Router);
    if(token==null){
        router.navigate([navigationRoute.login]);
    }
    return token!=null ? true : false;
};

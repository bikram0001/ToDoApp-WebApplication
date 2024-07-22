import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './services/interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
              provideAnimations(),
              provideToastr({timeOut:1000}),
              provideHttpClient(
                    withInterceptors([AuthInterceptor])
              ), provideAnimationsAsync(), provideAnimationsAsync()
            ] 
};

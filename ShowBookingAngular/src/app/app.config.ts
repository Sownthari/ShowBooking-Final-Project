import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { TheatreApiService } from './admin/theatre-api.service';
import { UserApiService } from './user/user-api.service';
import { AuthInterceptor } from './auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), 
    importProvidersFrom(HttpClientModule),
    TheatreApiService,
    UserApiService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }]
};

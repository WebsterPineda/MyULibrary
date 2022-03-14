import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const id_token = localStorage.getItem('id_token');

    if (id_token){
      const cloned = request.clone({
        headers: request.headers.set("Authorization", 'Bearer ' + id_token)
      });
      return next.handle(cloned);
    }
    return next.handle(request);
  }
}

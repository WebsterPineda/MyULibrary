import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_URL } from '../../shared/api.constants';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  _url = API_URL.concat('/api/login');
  header = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
  constructor(
    private http: HttpClient
  ) { }

  login(Email: string, Password: string){
    this.http.post(this._url, {Email, Password},{ headers: this.header, responseType: 'json'})
      .subscribe((req: any) => {
        if (req.token)
        {
          localStorage.setItem('token', req.token);
          localStorage.setItem('usr', Email);
        }
        else
        {
          localStorage.setItem('token', 'null');
          localStorage.setItem('usr', 'null');
        }
      });
  }

  userLogedIn(){
    const token = localStorage.getItem('token');
    return token !== 'null' ? true : false;
  }
}

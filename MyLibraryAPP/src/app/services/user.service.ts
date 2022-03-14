import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { API_URL } from '../../shared/api.constants';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  _url = API_URL.concat('/api/user');
  constructor(
    private http: HttpClient
  ) { 
    console.log('Servicio funcional');
  }

  getUser(){
    let header = new HttpHeaders()
      .set('Content-Type', 'application/json')

    return this.http.post(this._url, {
      Email: 'john.doe@gmail.com',
      Password: '123456'
      }, {
      headers: header,
      responseType: 'json'
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private loginService: LoginService) { }

  registerForm:any = FormGroup;
  submitted = false;
  title = "MyLibraryAPP";

  get f() { return this.registerForm.controls; }
  onSubmit() {
    this.submitted = true;
    if (this.registerForm.invalid){
      return;
    }
    if (this.submitted)
    {
      this.loginService.login(this.registerForm.controls.email, this.registerForm.controls.password);
      if(this.loginService.userLogedIn())
      {
        console.log('Welcome user');
        console.log(localStorage.getItem('token'));
      }
    }
  }

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

}

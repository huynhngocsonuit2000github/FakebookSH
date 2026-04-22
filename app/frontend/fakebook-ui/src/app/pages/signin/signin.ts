import { Component } from '@angular/core';
import { SigninForm } from './component/signin-form/signin-form';
import { SigninHero } from './component/signin-hero/signin-hero';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-signin',
  imports: [SigninForm, SigninHero, RouterModule],
  templateUrl: './signin.html',
  styleUrl: './signin.scss',
})
export class Signin {}

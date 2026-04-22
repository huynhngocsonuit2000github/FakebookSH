import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SigninHero } from '../signin/component/signin-hero/signin-hero';
import { SignupForm } from './component/signup-form/signup-form';

@Component({
  selector: 'app-signup',
  imports: [SignupForm, SigninHero, RouterModule],
  templateUrl: './signup.html',
  styleUrl: './signup.scss',
})
export class Signup {}

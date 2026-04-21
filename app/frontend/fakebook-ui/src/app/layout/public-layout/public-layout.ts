import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Header } from '../public-layout-component/header/header';
import { Footer } from '../public-layout-component/footer/footer';

@Component({
  selector: 'app-public-layout',
  imports: [RouterModule, Header, Footer],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.scss',
})
export class PublicLayout {}

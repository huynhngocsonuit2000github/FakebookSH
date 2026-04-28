import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Footer } from '../component/footer/footer';
import { PublicHeader } from '../component/public-header/public-header';

@Component({
  selector: 'app-public-layout',
  imports: [RouterModule, PublicHeader, Footer],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.scss',
})
export class PublicLayout {}

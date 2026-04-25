import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Header } from '../component/header/header';
import { LeftSidebar } from '../component/left-sidebar/left-sidebar';

@Component({
  selector: 'app-main-layout',
  imports: [RouterModule, Header, LeftSidebar],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.scss',
})
export class MainLayout {}

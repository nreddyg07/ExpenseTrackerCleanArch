import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MATERIAL_MODULES } from './material';
import { StagingService } from './services/staging.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet, ...MATERIAL_MODULES],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})

export class AppComponent {

  constructor(private staging: StagingService) {}

  openNewExpense() {
    this.staging.openDrawer$.next();
  }
  collapsed = false;

  toggleSidebar() {
    this.collapsed = !this.collapsed;
  }

}
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  model: any = {};
  // Creating an observable to check if the user is logged in. Since we can't assign a null to the observable we are using Union "<User | null>"
  // This still won't work since we still cannot assign the null value. We have to use the rxjs 'of' in order to point that the 
  // observable is an observable of type null
  currentUser$: Observable<User | null> = of(null);

  constructor(public accountService: AccountService) {} // If the service is private we won't be able to use it in the html template

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$; // This is another option if the service is private
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error)
    })
  }

  logout() {
    this.accountService.logout();
  }

}

import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {} // If the service is private we won't be able to use it in the html template

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$; // This is another option if the service is private
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/members'), // we could user () instead of the _ which means no argument is expected as the response
      //error: error => this.toastr.error(error.error) // Don't need this code. It is handled in the interceptor
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}

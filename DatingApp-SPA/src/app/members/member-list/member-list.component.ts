import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/user';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { UserService } from 'src/app/_services/user.service';


@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  user: User = JSON.parse(localStorage.getItem('user'));
  genderList = [{value: 'male', display: 'males'}, {value: 'female', display: 'females'}];
  userParams: any = {};


  constructor(private alertify: AlertifyService, private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = "lastActive"
    
    this.route.data.subscribe(data => {

      this.users = data.users.result;
      this.pagination = data.users.pagination
    });
  }

  pageChanged(event: any) {
    this.pagination.currentPage = event.page;
    this.loadUsers();

    
  }

  loadUsers() {
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams) 
        .subscribe( (data: PaginatedResult<User[]>) => {
          this.users = data.result;
          this.pagination = data.pagination;
        })

  }

  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = ""
    this.loadUsers();
  }


}

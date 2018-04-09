import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TableComponent } from './table/table.component';
import { ManageTableComponent } from './manage-table/manage-table.component'

const routes: Routes = [
  {
    path: '',
    redirectTo: 'tables',
    pathMatch: 'full'
  },
  {
    path: 'tables',
    component: TableComponent,
    data: {
      title: 'Tables'
    }
  },
  {
    path: 'tables/:id',
    component: ManageTableComponent,
    data: {
      title: 'Manage table'
    }
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrderRoutingModule {}

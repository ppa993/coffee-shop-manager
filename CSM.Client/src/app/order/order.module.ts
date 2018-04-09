import { NgModule } from '@angular/core';
import { SharedModule } from '@app/shared';
import { TableComponent } from './table/table.component';
import { ManageTableComponent } from './manage-table/manage-table.component'
import { OrderRoutingModule } from './order-routing.module'

@NgModule({
  imports: [SharedModule, OrderRoutingModule],
  declarations: [TableComponent, ManageTableComponent]
})
export class OrderModule {}

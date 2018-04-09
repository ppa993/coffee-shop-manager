import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'anms-move-table-dialog',
  templateUrl: './move-table-dialog.component.html',
  styleUrls: ['./move-table-dialog.component.scss']
})
export class MoveTableDialogComponent implements OnInit {

  tables = [1, 2, 3, 4, 5, 6, 7, 8, 9];

  constructor(
    public dialogRef: MatDialogRef<MoveTableDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

}

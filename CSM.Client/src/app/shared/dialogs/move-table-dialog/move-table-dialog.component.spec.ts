import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveTableDialogComponent } from './move-table-dialog.component';

describe('MoveTableDialogComponent', () => {
  let component: MoveTableDialogComponent;
  let fixture: ComponentFixture<MoveTableDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoveTableDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveTableDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

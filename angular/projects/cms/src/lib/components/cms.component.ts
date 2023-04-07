import { Component, OnInit } from '@angular/core';
import { CmsService } from '../services/cms.service';

@Component({
  selector: 'lib-cms',
  template: ` <p>cms works!</p> `,
  styles: [],
})
export class CmsComponent implements OnInit {
  constructor(private service: CmsService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}

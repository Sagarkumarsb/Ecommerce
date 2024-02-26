import { Component, Input } from '@angular/core';
import { Product } from 'src/app/shared/models/products';

@Component({
  selector: 'app-products-item',
  templateUrl: './products-item.component.html',
  styleUrls: ['./products-item.component.scss']
})
export class ProductsItemComponent {
@Input() product? : Product;
}

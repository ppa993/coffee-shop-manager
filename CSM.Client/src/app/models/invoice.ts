import {Product, ProductCollection} from './product';

export class Invoice {
    id: number;
    tableID: number;
    total: number;
    products: ProductCollection[];
    createdDate: Date;
}  
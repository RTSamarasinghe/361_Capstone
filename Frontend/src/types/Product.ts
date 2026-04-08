export interface Product {
  id: number;
  name: string;
  description : string;
  price: number;
  categoryId: number;
  imageUrl: string;
  manufacturuer: string;
  rating: number;
  sku: string;
  stockQuantity: number;
}
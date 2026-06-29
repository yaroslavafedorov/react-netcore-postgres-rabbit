export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  categoryId: string;
}

export interface Category {
  id: string;
  name: string;
  description: string;
}

export interface ProductsResponse {
  total: number;
  items: Product[];
  skip: number;
  take: number;
}

// Элемент корзины: сам продукт + его количество
export interface CartItem {
  product: Product;
  quantity: number;
}
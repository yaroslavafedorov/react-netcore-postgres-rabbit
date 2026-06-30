export interface OrderItem {
  productId: string;
  quantity: number;
}

export interface OrderCreateResponse {
  id: string;
  status: string;
}

import type { ProductsResponse, Category, Product } from '../types/product';

const BASE_URL = 'http://localhost:5030/api';

export const catalogApi = {

  getCategories: async (skip = 0, take = 10): Promise<Category[]> => {
    const params = new URLSearchParams();
    if (skip !== null && skip !== undefined)
      params.append('skip', skip.toString());
    if (take !== null && take !== undefined)
      params.append('take', take.toString());

    const response = await fetch(`${BASE_URL}/categories?${params.toString()}`);
    if (!response.ok)
      throw new Error('Ошибка загрузка товаров');

    const items: Category[] = await response.json();

    return items;
  },

  getProducts: async (categoryId?: string, skip = 0, take = 10): Promise<ProductsResponse> => {
    const params = new URLSearchParams();
    
    if (categoryId !== null && categoryId !== undefined)
      params.append('categoryId', categoryId.toString());
    if (skip !== null && skip !== undefined)
      params.append('skip', skip.toString());
    if (take !== null && take !== undefined)
      params.append('take', take.toString());

    const response = await fetch(`${BASE_URL}/products?${params.toString()}`);
    if (!response.ok)
      throw new Error('Ошибка загрузка товаров');

    const items: Product[] = await response.json();

    return {
      total: items.length, // Временное решение, пока бэкенд не возвращает Total count
      items: items,
      skip,
      take
    };
  },

  getProductById: async (id: string): Promise<Product | undefined> => {
    const response = await fetch(`${BASE_URL}/products/${id}`);
    if (response.status == 404)
      return undefined;
    if (!response.ok)
      throw new Error('Ошибка загрузки карточки товара ${id}');
    return response.json();
  }
};

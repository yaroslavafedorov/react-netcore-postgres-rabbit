import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { catalogApi } from '../api/catalogApi';
import type { Product } from '../types/product';

interface ProductListProps {
  addToCart: (product: Product) => void;
}

export function ProductList({ addToCart }: ProductListProps) {
  const { categoryId } = useParams<{ categoryId?: string }>();
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    catalogApi.getProducts(categoryId).then(data => setProducts(data.items));
  }, [categoryId]);

  return (
    <div>
      <h2>{categoryId ? "Товары в категории" : "Все товары каталога"}</h2>
      <div style={{ display: 'flex', gap: '15px', flexWrap: 'wrap', marginTop: '20px' }}>
        {products.map(product => (
          <div key={product.id} style={{ border: '1px solid #555', padding: '15px', borderRadius: '8px', backgroundColor: '#222', minWidth: '180px' }}>
            <Link to={`/products/${product.id}`} style={{ color: '#646cff', textDecoration: 'none' }}>
              <h3>{product.name}</h3>
            </Link>
            <p style={{ fontWeight: 'bold' }}>{product.price} руб.</p>
            <button onClick={() => addToCart(product)} style={{ cursor: 'pointer', padding: '5px 10px' }}>
              Купить
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}

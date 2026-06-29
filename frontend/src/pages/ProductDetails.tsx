import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { catalogApi } from '../api/catalogApi';
import type { Product } from '../types/product';

interface ProductDetailsProps {
  addToCart: (product: Product) => void;
}

export function ProductDetails({ addToCart }: ProductDetailsProps) {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [product, setProduct] = useState<Product | null>(null);

  useEffect(() => {
    if (id) {
      catalogApi.getProductById(id).then(p => setProduct(p || null));
    }
  }, [id]);

  if (!product) return <p>Товар не найден или загружается...</p>;

  return (
    <div style={{ padding: '20px', border: '1px solid #555', borderRadius: '8px', backgroundColor: '#222', marginTop: '20px', textAlign: 'left' }}>
      <button onClick={() => navigate(-1)} style={{ marginBottom: '15px', cursor: 'pointer' }}>← Назад</button>
      <h2>{product.name}</h2>
      <p style={{ color: '#aaa', margin: '15px 0' }}>{product.description}</p>
      <h3 style={{ color: '#646cff' }}>Цена: {product.price} руб.</h3>
      <button onClick={() => addToCart(product)} style={{ padding: '10px 20px', fontSize: '16px', marginTop: '15px', cursor: 'pointer' }}>
        Добавить в корзину
      </button>
    </div>
  );
}

import type { Product } from '../types/product';

// Описываем форму входных параметров для карточки
interface ProductCardProps {
  product: Product;
}

export function ProductCard({ product }: ProductCardProps) {
  return (
    <div style={{ 
      border: '1px solid #555', 
      padding: '15px', 
      borderRadius: '8px', 
      minWidth: '150px', 
      backgroundColor: '#222' 
    }}>
      <h3>{product.name}</h3>
      <p style={{ color: '#646cff', fontWeight: 'bold' }}>{product.price} руб.</p>
      <button 
        type="button" 
        className="counter" 
        style={{ padding: '5px 10px', fontSize: '14px', cursor: 'pointer' }}
      >
        Купить
      </button>
    </div>
  );
}

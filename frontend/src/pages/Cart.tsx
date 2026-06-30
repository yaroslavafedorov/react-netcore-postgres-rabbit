import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { orderApi } from '../api/orderApi';
import type { CartItem } from '../types/product';
import type { OrderItem } from '../types/order';

interface CartProps {
  cartItems: CartItem[];
  clearCart: () => void;
  onOrderSuccess: (orderId: string) => void
}

export function Cart({ cartItems, clearCart, onOrderSuccess }: CartProps) {
  const totalPrice = cartItems.reduce((sum, item) => sum + item.product.price * item.quantity, 0);
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleCheckout()
  {
      setIsSubmitting(true);

      const items: OrderItem[] = cartItems.map(item => ({
        productId: item.product.id, // Достаем вложенный ID и переименовываем в productId
        quantity: item.quantity     // Переносим количество штук
      }));

      try
      {
        const result = await orderApi.createOrder(items);
        clearCart();
        onOrderSuccess(result);

        return result;
      }
      catch
      {
        alert('Ошибка оформления заказа');
      }
      finally
      {
        setIsSubmitting(false);
      }
  }

  return (
    <div>
      <h2>Ваша корзина</h2>
      {cartItems.length === 0 ? (
        <p style={{ marginTop: '20px' }}>Корзина пуста. Начните с <Link to="/" style={{ color: '#646cff' }}>каталога</Link>.</p>
      ) : (
        <div style={{ marginTop: '20px', textAlign: 'left' }}>
          <table style={{ width: '100%', borderCollapse: 'collapse', backgroundColor: '#222' }}>
            <thead>
              <tr style={{ borderBottom: '1px solid #555' }}>
                <th style={{ padding: '10px' }}>Товар</th>
                <th style={{ padding: '10px' }}>Цена</th>
                <th style={{ padding: '10px' }}>Количество</th>
                <th style={{ padding: '10px' }}>Сумма</th>
              </tr>
            </thead>
            <tbody>
              {cartItems.map(item => (
                <tr key={item.product.id} style={{ borderBottom: '1px solid #444' }}>
                  <td style={{ padding: '10px' }}>
                    <Link to={`/products/${item.product.id}`} style={{ color: '#646cff', textDecoration: 'none' }}>
                      {item.product.name}
                    </Link>
                  </td>
                  <td style={{ padding: '10px' }}>{item.product.price} руб.</td>
                  <td style={{ padding: '10px' }}>{item.quantity} шт.</td>
                  <td style={{ padding: '10px' }}>{item.product.price * item.quantity} руб.</td>
                </tr>
              ))}
            </tbody>
          </table>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '20px' }}>
            <h3>Итого: <span style={{ color: '#646cff' }}>{totalPrice} руб.</span></h3>
            <button onClick={clearCart} style={{ backgroundColor: '#d9534f', color: 'white', padding: '10px 15px', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>
              Очистить корзину
            </button>
            <button onClick={handleCheckout} style={{ backgroundColor: '#684fd9', color: 'white', padding: '10px 15px', border: 'none', borderRadius: '4px', cursor: 'pointer' }} disabled={isSubmitting}>
              Оформить заказ
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

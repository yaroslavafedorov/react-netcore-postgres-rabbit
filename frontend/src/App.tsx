import { useState } from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { CategoryList } from './pages/CategoryList';
import { ProductList } from './pages/ProductList';
import { ProductDetails } from './pages/ProductDetails';
import { Cart } from './pages/Cart';
import type { Product, CartItem } from './types/product';
import './App.css';

function App() {
  // Глобальное состояние корзины на верхнем уровне
  const [cart, setCart] = useState<CartItem[]>([]);

  // Функция добавления товара в корзину
  const addToCart = (product: Product) => {
    setCart(prevCart => {
      const existing = prevCart.find(item => item.product.id === product.id);
      if (existing) {
        return prevCart.map(item => 
          item.product.id === product.id ? { ...item, quantity: item.quantity + 1 } : item
        );
      }
      return [...prevCart, { product, quantity: 1 }];
    });
  };

  const clearCart = () => setCart([]);
  
  // Общее количество товаров для иконки в шапке
  const totalItemsInCart = cart.reduce((sum, item) => sum + item.quantity, 0);

  return (
    <BrowserRouter>
      {/* Навигационная шапка сайта */}
      <nav style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        padding: '15px 30px', 
        borderBottom: '1px solid #555', 
        backgroundColor: '#1a1a1a',
        marginBottom: '30px'
      }}>
        <div style={{ display: 'flex', gap: '20px', fontSize: '18px' }}>
          <Link to="/" style={{ color: '#fff', textDecoration: 'none', fontWeight: 'bold' }}>Магазин</Link>
          <Link to="/categories" style={{ color: '#aaa', textDecoration: 'none' }}>Категории</Link>
          <Link to="/products" style={{ color: '#aaa', textDecoration: 'none' }}>Все товары</Link>
        </div>
        <div>
          <Link to="/cart" style={{ color: '#646cff', textDecoration: 'none', fontWeight: 'bold' }}>
            🛒 Корзина ({totalItemsInCart})
          </Link>
        </div>
      </nav>

      {/* Контентная область страниц */}
      <main style={{ maxWidth: '1200px', margin: '0 auto', padding: '0 20px' }}>
        <Routes>
          {/* Главная страница по умолчанию ведет на категории */}
          <Route path="/" element={<CategoryList />} />
          <Route path="/categories" element={<CategoryList />} />
          <Route path="/categories/:categoryId" element={<ProductList addToCart={addToCart} />} />
          <Route path="/products" element={<ProductList addToCart={addToCart} />} />
          <Route path="/products/:id" element={<ProductDetails addToCart={addToCart} />} />
          <Route path="/cart" element={<Cart cartItems={cart} clearCart={clearCart} />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;

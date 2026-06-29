import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { catalogApi } from '../api/catalogApi';
import type { Category } from '../types/product';

export function CategoryList() {
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    catalogApi.getCategories().then(setCategories);
  }, []);

  return (
    <div>
      <h2>Категории товаров</h2>
      <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
        {categories.map(cat => (
          <Link to={`/categories/${cat.id}`} key={cat.id} style={{ textDecoration: 'none', color: 'inherit' }}>
            <div style={{ border: '1px solid #555', padding: '20px', borderRadius: '8px', backgroundColor: '#222', cursor: 'pointer' }}>
              <h3>{cat.name}</h3>
              <p style={{ color: '#aaa' }}>{cat.description}</p>
            </div>
          </Link>
        ))}
      </div>
    </div>
  );
}

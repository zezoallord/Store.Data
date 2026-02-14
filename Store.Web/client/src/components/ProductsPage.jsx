import React, { useEffect, useState } from 'react';
import { getProducts, getBasket, updateBasket } from '../api/api';
import ProductCard from './ProductCard';

export default function ProductsPage() {
    const [products, setProducts] = useState([]);
    const [search, setSearch] = useState('');
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadProducts();
    }, [search]); // Reload when search changes (basic implementation)

    const loadProducts = async () => {
        setLoading(true);
        try {
            const data = await getProducts({ search });
            setProducts(data || []);
        } catch (error) {
            console.error('Failed to load products', error);
        } finally {
            setLoading(false);
        }
    };

    const addToBasket = async (product) => {
        const basketId = localStorage.getItem('basket_id') || 'basket_' + Math.random().toString(36).substr(2, 9);
        localStorage.setItem('basket_id', basketId);

        try {
            let basket = await getBasket(basketId);
            if (!basket) {
                basket = { id: basketId, basketItems: [] };
            }

            const existingItem = basket.basketItems.find(i => i.productId === product.id);
            if (existingItem) {
                existingItem.quantity += 1;
            } else {
                basket.basketItems.push({
                    productId: product.id,
                    productName: product.name,
                    price: product.price,
                    quantity: 1,
                    pictureUrl: product.pictureUrl,
                    brandName: product.brandName,
                    typeName: product.typeName
                });
            }

            await updateBasket(basket);
            alert('Product added to basket!');
        } catch (error) {
            console.error('Error updating basket', error);
            alert('Failed to add to basket');
        }
    };

    return (
        <div className="products-page">
            <div className="search-bar">
                <input
                    type="text"
                    placeholder="Search products..."
                    value={search}
                    onChange={(e) => setSearch(e.target.value)}
                />
            </div>

            {loading ? (
                <div>Loading...</div>
            ) : (
                <div className="product-grid">
                    {products.map(product => (
                        <ProductCard
                            key={product.id}
                            product={product}
                            onAddToBasket={addToBasket}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}

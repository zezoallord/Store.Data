import React, { useEffect, useState } from 'react';
import { getBasket, updateBasket, deleteBasket } from '../api/api';

export default function BasketPage() {
    const [basket, setBasket] = useState(null);

    useEffect(() => {
        loadBasket();
    }, []);

    const loadBasket = async () => {
        const basketId = localStorage.getItem('basket_id');
        if (!basketId) return;
        try {
            const data = await getBasket(basketId);
            setBasket(data);
        } catch (error) {
            console.error('Failed to load basket', error);
        }
    };

    const removeItem = async (itemId) => {
        if (!basket) return;
        const updatedItems = basket.basketItems.filter(i => i.productId !== itemId);
        const updatedBasket = { ...basket, basketItems: updatedItems };

        try {
            if (updatedItems.length === 0) {
                await deleteBasket(basket.id);
                setBasket(null);
                localStorage.removeItem('basket_id');
            } else {
                await updateBasket(updatedBasket);
                setBasket(updatedBasket);
            }
        } catch (error) {
            console.error('Failed to remove item', error);
        }
    };

    if (!basket || basket.basketItems.length === 0) return <div>Your basket is empty.</div>;

    const total = basket.basketItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);

    return (
        <div className="basket-page">
            <h2>Shopping Basket</h2>
            <div className="basket-items">
                {basket.basketItems.map(item => (
                    <div key={item.productId} className="basket-item">
                        <img src={item.pictureUrl} alt={item.productName} width="50" />
                        <div className="item-details">
                            <h4>{item.productName}</h4>
                            <p>Quantity: {item.quantity}</p>
                            <p>Price: ${item.price}</p>
                        </div>
                        <button onClick={() => removeItem(item.productId)}>Remove</button>
                    </div>
                ))}
            </div>
            <div className="basket-summary">
                <h3>Total: ${total.toFixed(2)}</h3>
            </div>
        </div>
    );
}

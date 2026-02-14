import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import ProductsPage from './components/ProductsPage';
import LoginPage from './components/LoginPage';
import RegisterPage from './components/RegisterPage';
import BasketPage from './components/BasketPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<ProductsPage />} />
          <Route path="products" element={<ProductsPage />} />
          <Route path="login" element={<LoginPage />} />
          <Route path="register" element={<RegisterPage />} />
          <Route path="basket" element={<BasketPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;

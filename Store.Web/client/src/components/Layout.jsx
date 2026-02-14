import React, { useEffect, useState } from 'react';
import { Link, Outlet, useNavigate, useLocation } from 'react-router-dom';
import { logout } from '../api/api';

export default function Layout() {
    const [displayName, setDisplayName] = useState('');
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        const name = localStorage.getItem('displayName');
        setDisplayName(name || '');
    }, [location]); // Re-check on navigation (e.g. after login)

    const handleLogout = async () => {
        await logout();
        setDisplayName('');
    };

    return (
        <div className="app-container">
            <nav className="navbar">
                <div className="nav-brand">Store</div>
                <div className="nav-links">
                    <Link to="/">Products</Link>
                    <Link to="/basket">Basket</Link>

                    {displayName ? (
                        <>
                            <span style={{ marginLeft: '1.5rem', fontWeight: 'bold' }}>Hello, {displayName}</span>
                            <button
                                onClick={handleLogout}
                                style={{
                                    marginLeft: '1rem',
                                    background: 'transparent',
                                    color: '#ef4444',
                                    border: '1px solid #ef4444',
                                    padding: '0.4rem 0.8rem',
                                    width: 'auto'
                                }}
                            >
                                Logout
                            </button>
                        </>
                    ) : (
                        <Link to="/login">Login</Link>
                    )}
                </div>
            </nav>
            <main className="main-content">
                <Outlet />
            </main>
        </div>
    );
}

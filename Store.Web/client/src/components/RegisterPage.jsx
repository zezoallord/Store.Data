import React, { useState } from 'react';
import { register } from '../api/api';
import { useNavigate, Link } from 'react-router-dom';

export default function RegisterPage() {
    const [displayName, setDisplayName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        try {
            await register(displayName, email, password);
            navigate('/');
        } catch (err) {
            // Improve error message friendliness
            let msg = err.message;
            if (msg.includes("400")) msg = "Please check your details. Password needs to be strong (1 Upper, 1 Lower, 1 Number, 1 Special char).";
            if (msg.includes("taken")) msg = "This email is already in use.";
            setError(msg);
        }
    };

    return (
        <div className="auth-container">
            <div className="card auth-card">
                <h2>Create Account</h2>
                <p className="auth-subtitle">Join us to start shopping</p>

                {error && <div className="error-message">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label>Display Name</label>
                        <input
                            type="text"
                            value={displayName}
                            onChange={e => setDisplayName(e.target.value)}
                            required
                            placeholder="Your Name"
                        />
                    </div>
                    <div className="form-group">
                        <label>Email</label>
                        <input
                            type="email"
                            value={email}
                            onChange={e => setEmail(e.target.value)}
                            required
                            placeholder="you@example.com"
                        />
                    </div>
                    <div className="form-group">
                        <label>Password</label>
                        <input
                            type="password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                            required
                            placeholder="Strong Password"
                        />
                        <small className="hint">Must have 1 Uppercase, 1 Lowercase, 1 Number, 1 Special char.</small>
                    </div>
                    <button type="submit" className="btn-primary">Sign Up</button>
                </form>
                <p className="auth-footer">
                    Already have an account? <Link to="/login">Login here</Link>
                </p>
            </div>
        </div>
    );
}

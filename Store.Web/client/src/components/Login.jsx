import React, { useState } from 'react'
import { login, getToken, logout } from '../api/api'

export default function Login({ onLogin }){
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const submit = async (e) =>{
    e.preventDefault()
    setLoading(true)
    setError(null)
    try{
      await login(email, password)
      onLogin && onLogin()
    }catch(err){
      setError(err.message || 'Login failed')
    }finally{ setLoading(false) }
  }

  const handleLogout = () => {
    logout()
    onLogin && onLogin()
  }

  if (getToken()){
    return (
      <div>
        <p>Signed in</p>
        <button onClick={handleLogout}>Sign out</button>
      </div>
    )
  }

  return (
    <form onSubmit={submit} style={{maxWidth:420}}>
      <h3>Sign in</h3>
      {error && <div style={{color:'red'}}>{error}</div>}
      <div style={{marginBottom:8}}>
        <input placeholder="Email" value={email} onChange={e=>setEmail(e.target.value)} style={{width:'100%',padding:8}} />
      </div>
      <div style={{marginBottom:8}}>
        <input placeholder="Password" type="password" value={password} onChange={e=>setPassword(e.target.value)} style={{width:'100%',padding:8}} />
      </div>
      <div>
        <button type="submit" disabled={loading}>{loading? 'Signing...' : 'Sign in'}</button>
      </div>
    </form>
  )
}

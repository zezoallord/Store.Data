import React, { useState } from 'react'
import ProductList from './components/ProductList'
import Login from './components/Login'
import { getToken } from './api/api'

export default function App(){
  const [authToggle, setAuthToggle] = useState(0)
  const triggerRefresh = () => setAuthToggle(t => t + 1)

  return (
    <div>
      <header style={{background:'#1f6feb',color:'#fff',padding:12}}> 
        <h1 style={{margin:0}}>Store</h1>
      </header>
      <main style={{padding:18}}>
        <div style={{display:'flex',gap:20}}>
          <div style={{flex:1}}>
            <Login onLogin={triggerRefresh} />
          </div>
          <div style={{flex:3}}>
            <ProductList key={authToggle} />
          </div>
        </div>
      </main>
      <footer style={{padding:12,textAlign:'center',color:'#666'}}>Built with React + Vite</footer>
    </div>
  )
}

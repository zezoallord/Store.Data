import React from 'react'
import ProductList from './components/ProductList'

export default function App(){
  return (
    <div>
      <header style={{background:'#1f6feb',color:'#fff',padding:12}}> 
        <h1 style={{margin:0}}>Store</h1>
      </header>
      <main style={{padding:18}}>
        <ProductList />
      </main>
      <footer style={{padding:12,textAlign:'center',color:'#666'}}>Built with React + Vite</footer>
    </div>
  )
}

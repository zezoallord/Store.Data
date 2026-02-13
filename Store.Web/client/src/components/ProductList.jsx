import React, {useEffect, useState} from 'react'
import ProductCard from './ProductCard'
import { getProducts } from '../api/api'

export default function ProductList(){
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(()=>{
    setLoading(true)
    getProducts()
      .then(data => setProducts(data || []))
      .catch(err => setError(err.message || 'Failed to load'))
      .finally(()=>setLoading(false))
  },[])

  if(loading) return <div className="loading">Loading...</div>
  if(error) return <div style={{color:'red'}}>Error: {error}</div>

  return (
    <div className="grid">
      {products.length === 0 && <div>No products found</div>}
      {products.map(p => (
        <ProductCard key={p.id} product={p} />
      ))}
    </div>
  )
}

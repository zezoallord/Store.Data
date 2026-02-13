import React from 'react'

export default function ProductCard({product}){
  return (
    <div className="card">
      <img src={product.pictureUrl || '/placeholder.png'} alt={product.name} />
      <h3>{product.name}</h3>
      <p>{product.brand} • {product.type}</p>
      <div style={{marginTop:'auto',display:'flex',justifyContent:'space-between',alignItems:'center'}}>
        <div className="price">${(product.price || 0).toFixed(2)}</div>
        <button>Add</button>
      </div>
    </div>
  )
}

const API_BASE = import.meta.env.VITE_API_BASE || 'http://localhost:5032'

export async function getProducts(){
  const url = `${API_BASE}/api/products/getallproducts`
  const res = await fetch(url)
  if(!res.ok) throw new Error(`Network response was not ok: ${res.status} ${res.statusText} (url: ${url})`)
  return res.json()
}
